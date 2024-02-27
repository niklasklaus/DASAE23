using System.Globalization;

namespace DA_PDF_Generierung_Result;

public class PriceCalculator
{
    public static double SubLvPrice = 0.0;
    public static double LvPrice = 0.0;
    public static double TotalPriceOfLv = 0.0;
    public static string CurrentMainOzNumberLV = "";
    public static string CurrentSubOzNumberLV = "";
    public static double test = 0.0;


    
    public static void CalculatePrice(int proposalId, int userId)
    {
        MySqlConnector connector = new MySqlConnector();
        
        // Get necessary Data from Darabase
        List<Dictionary<string, object>> porposalData = connector.RetrieveDataListFromDatabase(connector.GetConeection(), $"SELECT * FROM PROPOSALS WHERE proposal_id = {proposalId} AND user_id = {userId}");
        List<Dictionary<string, object>> dataLVsFromDatabase = connector.RetrieveDataListFromDatabase(connector.GetConeection(), $"SELECT * FROM LVS WHERE proposal_id = {proposalId} AND user_id = {userId}");
        
        string dicountSave = (porposalData[0].TryGetValue("discount", out object discountObject) && discountObject != null) ? Convert.ToString(discountObject) : null;
        
        //Überprüfung ob Discount null ist => Wert auf 0.00 setzen
        if (dicountSave == "")
        {
            Console.WriteLine("Is nUll Methode");
            ProgramLogic.DiscountValue = "0,00";
            Console.WriteLine(Convert.ToDouble(ProgramLogic.DiscountValue));
        }
        else
        {
            ProgramLogic.DiscountValue = dicountSave;
        }

        for (int i = 0; i < dataLVsFromDatabase.Count; i++)
        {
            string ozNumberPrevious = "";
            if (i > 0)
            {
                ozNumberPrevious = (dataLVsFromDatabase[i-1].TryGetValue("oz", out object ozNumberPreviousObject) && ozNumberPreviousObject != null) ? Convert.ToString(ozNumberPreviousObject) : null;
            }
            
            string ozNumber = (dataLVsFromDatabase[i].TryGetValue("oz", out object ozNumberObject) && ozNumberObject != null) ? Convert.ToString(ozNumberObject) : null;
            string ep_basic = (dataLVsFromDatabase[i].TryGetValue("basic_ep", out object ep_basicObject) && ep_basicObject != null) ? Convert.ToString(ep_basicObject) : null;
            string basic_gb = (dataLVsFromDatabase[i].TryGetValue("calculated_gb", out object basic_gbObject) && basic_gbObject != null) ? Convert.ToString(basic_gbObject) : null;

            
            //01.
            if (ProgramLogic.IsOZStructureValid1(ozNumber.Trim()))
            {
                //01.01.01.
                if (ProgramLogic.IsOZStructureValid3(ozNumberPrevious.Trim()))
                {
                    Console.WriteLine($"LvPrice: {LvPrice} - SubLvPrice: {SubLvPrice}");
                    connector.UpdateDataFromDatabase(connector.GetConeection(), $"UPDATE LVS SET calculated_gb = CAST(REPLACE('{SubLvPrice}', ',', '.') AS DECIMAL(10, 2)) WHERE oz = '{CurrentSubOzNumberLV}' AND proposal_id = '{proposalId}' AND user_id = '{userId}'");
                    connector.UpdateDataFromDatabase(connector.GetConeection(), $"UPDATE LVS SET calculated_gb = CAST(REPLACE('{LvPrice}', ',', '.') AS DECIMAL(10, 2)) WHERE oz = '{CurrentMainOzNumberLV}' AND proposal_id = '{proposalId}' AND user_id = '{userId}'");
                    SubLvPrice = 0.0;
                    LvPrice = 0.0;
                }

                CurrentMainOzNumberLV = ozNumber;
            }
            //02.02.
            else if(ProgramLogic.IsOZStructureValid2(ozNumber.Trim()))
            {
                if (ProgramLogic.IsOZStructureValid3(ozNumberPrevious.Trim()))
                {
                    Console.WriteLine($"SubLvPrice: {SubLvPrice}");
                    connector.UpdateDataFromDatabase(connector.GetConeection(), $"UPDATE LVS SET calculated_gb = CAST(REPLACE('{SubLvPrice}', ',', '.') AS DECIMAL(10, 2)) WHERE oz = '{CurrentSubOzNumberLV}' AND proposal_id = '{proposalId}' AND user_id = '{userId}'");
                    SubLvPrice = 0.0;
                }
                CurrentSubOzNumberLV = ozNumber;
            }
            //01.01.01.
            else if (ProgramLogic.IsOZStructureValid3(ozNumber.Trim()))
            {
                SubLvPrice += Convert.ToDouble(basic_gb);
                LvPrice += Convert.ToDouble(basic_gb);
                TotalPriceOfLv += Convert.ToDouble(basic_gb);
            }
            
            // Ausgabe letzter TotalSum
            if (i + 1 == dataLVsFromDatabase.Count)
            {
                Console.WriteLine($"LvPrice: {LvPrice} - SubLvPrice: {SubLvPrice}");
                connector.UpdateDataFromDatabase(connector.GetConeection(), $"UPDATE LVS SET calculated_gb = '{SubLvPrice.ToString("0.00", CultureInfo.InvariantCulture)}' WHERE oz = '{CurrentSubOzNumberLV}' AND proposal_id = '{proposalId}' AND user_id = '{userId}'");
                connector.UpdateDataFromDatabase(connector.GetConeection(), $"UPDATE LVS SET calculated_gb = '{LvPrice.ToString("0.00", CultureInfo.InvariantCulture)}' WHERE oz = '{CurrentMainOzNumberLV}' AND proposal_id = '{proposalId}' AND user_id = '{userId}'");
                SubLvPrice = 0.0;
                LvPrice = 0.0;
            }
        }
        
        //connector.UpdateDataFromDatabase(connector.GetConeection(), $"UPDATE PROPOSALS SET proposal_price = '{TotalPriceOfLv.ToString("0.00", CultureInfo.InvariantCulture)}' WHERE proposal_id = '{proposalId}' AND user_id = '{userId}'");

        TotalPriceOfLv = 0.0;
    }
}