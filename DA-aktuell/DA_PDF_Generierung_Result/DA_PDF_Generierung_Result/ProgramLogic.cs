using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;

namespace DA_PDF_Generierung_Result;

public class ProgramLogic
{
    public static List<List<Tuple<string, string, double>>> overviewList = new List<List<Tuple<string, string, double>>>();
    public static List<Tuple<string, string, double>> innerList = new List<Tuple<string, string, double>>();
    public static Tuple<string, string, double> tuple;
    public static int yPos = 546;
    public static int PageCnt = 0;
    public static double SubLvPrice = 0.0;
    public static double LvPrice = 0.0;
    public static string CurrentMainOzNumberLV = "";
    public static string CurrentSubOzNumberLV = "";
    public static double TupleSubLvPrice = 0.0;
    public static double TupleLvPrice = 0.0;
    public static int proposalId;
    public static string LvName = "";
    public static double TotalPriceOfLv = 0.0;
    public static string DiscountValue = "0,00";
    public static int userId;

    public static void ProgramLogicWorkflow(string destPdfPath)
    {
        MySqlConnector connector = new MySqlConnector();
        
        string sourcePdfPath = $"Kleinangebote_LV_Vorlage_Form_Delete.pdf";
        
        // Get necessary Data from Darabase
        List<Dictionary<string, object>> porposalData = connector.RetrieveDataListFromDatabase(connector.GetConeection(), $"SELECT * FROM PROPOSALS WHERE proposal_id = {proposalId} AND user_id = {userId}");
        List<Dictionary<string, object>> dataLVsFromDatabase = connector.RetrieveDataListFromDatabase(connector.GetConeection(), $"SELECT * FROM LVS WHERE proposal_id = {proposalId} AND user_id = {userId}");

        string dicountSave = (porposalData[0].TryGetValue("discount", out object discountObject) && discountObject != null) ? Convert.ToString(discountObject) : null;
        
        using (PdfDocument pdfDocument = new PdfDocument(new PdfReader(sourcePdfPath), new PdfWriter(destPdfPath)))
        {
            
            PageCnt+=2;
            
            string CurrentMainNumber = "";
            string CurrentMainShort_Text = "";
            string CurrentSubNumber = "";
            string CurrentSubShort_Text = "";
            
            
            string PreviousMainNumber = "";
            string PreviousShort_Text = "";
            string PreviousSubNumber = "";
            string PreviousSubShort_Text = "";
            
            
            for (int i = 0; i < dataLVsFromDatabase.Count; i++)
            {
                //Console.WriteLine($"Current Index {i}");
                string ozNumberPrevious = "";
                string short_textPrevious = "";
                
                if (i > 0)
                {
                    ozNumberPrevious = (dataLVsFromDatabase[i-1].TryGetValue("oz", out object ozNumberPreviousObject) && ozNumberPreviousObject != null) ? Convert.ToString(ozNumberPreviousObject) : null;
                    short_textPrevious = (dataLVsFromDatabase[i-1].TryGetValue("short_text", out object short_textPreviousObject) && short_textPreviousObject != null) ? Convert.ToString(short_textPreviousObject) : null;
                }
                
                string ozNumber = (dataLVsFromDatabase[i].TryGetValue("oz", out object ozNumberObject) && ozNumberObject != null) ? Convert.ToString(ozNumberObject) : null;
                string short_text = (dataLVsFromDatabase[i].TryGetValue("short_text", out object short_textObject) && short_textObject != null) ? Convert.ToString(short_textObject) : null;
                string long_text = (dataLVsFromDatabase[i].TryGetValue("long_text", out object long_textObject) && long_textObject != null) ? Convert.ToString(long_textObject) : null;
                string lv_amount = (dataLVsFromDatabase[i].TryGetValue("lv_amount", out object lv_amountObject) && lv_amountObject != null) ? Convert.ToDouble(lv_amountObject).ToString("0.00") : null;
                string ep_basic = (dataLVsFromDatabase[i].TryGetValue("basic_ep", out object ep_basicObject) && ep_basicObject != null) ? Convert.ToString(ep_basicObject) : null;
                string lv_amount_unit = (dataLVsFromDatabase[i].TryGetValue("lv_amount_unit", out object lv_amount_unitObject) && lv_amount_unitObject != null) ? Convert.ToString(lv_amount_unitObject) : null;
                double calculated_gb = (dataLVsFromDatabase[i].TryGetValue("calculated_gb", out object basic_gbObject) && basic_gbObject != null) ? Convert.ToDouble(basic_gbObject) : 0.0;
                LvName = (dataLVsFromDatabase[i].TryGetValue("lv_type", out object lv_typeObject) && lv_typeObject != null) ? Convert.ToString(lv_typeObject) : null;
                
                //Get TotalPrice from Proposal Table
                TotalPriceOfLv = (porposalData[0].TryGetValue("proposal_price", out object proposal_priceObject) && proposal_priceObject != null) ? Convert.ToDouble(proposal_priceObject) : 0.0;
                
                //Überprüfung ob OzNummer xx. Struktur hat
                if(IsOZStructureValid1(ozNumber.Trim()) && Convert.ToDouble(calculated_gb) > 0)
                {
                    if (IsOZStructureValid3(ozNumberPrevious.Trim()))
                    {
                        //Überprüfung ob SummRow eingefügt werden muss
                        innerList.Add(new Tuple<string, string, double>(CurrentSubNumber.ToString(), CurrentSubShort_Text, SubLvPrice));
                        innerList.Add(new Tuple<string, string, double>(CurrentMainNumber.ToString(), CurrentMainShort_Text, LvPrice));
                        Console.WriteLine(CurrentMainNumber);
                        InsertSubSumRow(pdfDocument, CurrentSubNumber, CurrentSubShort_Text, 0, i);
                        InsertSumRow(pdfDocument, CurrentMainNumber, CurrentMainShort_Text, 0, i);
                        overviewList.Add(innerList);

                    }
                    
                    innerList = new List<Tuple<string, string, double>>();
                    
                    InsertHeaderRow(pdfDocument, ozNumber, short_text, i);
                    if (string.IsNullOrWhiteSpace(long_text))
                    {
                        //yPos -= 58;
                    }
                    else
                    {
                        //InsertMethod long_text
                    }

                    CurrentMainNumber = ozNumber;
                    CurrentMainShort_Text = short_text;
                    
                    LvPrice = calculated_gb;
                    CurrentMainOzNumberLV = ozNumber;
                }
                else if(IsOZStructureValid2(ozNumber) && Convert.ToDouble(calculated_gb) > 0)
                {
                    if (IsOZStructureValid3(ozNumberPrevious.Trim()) && SubLvPrice > 0)
                    {
                        innerList.Add(new Tuple<string, string, double>(CurrentSubNumber.ToString(), CurrentSubShort_Text, SubLvPrice));
                        Console.WriteLine(CurrentMainNumber);
                        InsertSubSumRow(pdfDocument, CurrentSubNumber, CurrentSubShort_Text, 0, i);
                    }
                    
                    InsertHeaderRow(pdfDocument, ozNumber, short_text, i);
                    if (string.IsNullOrWhiteSpace(long_text))
                    {
                        
                    }
                    else
                    {
                        //InsertMethod long_text
                    }
                    if (IsOZStructureValid3(ozNumberPrevious))
                    {
                        
                    }
                    CurrentSubNumber = ozNumber;
                    CurrentSubShort_Text = short_text;
                    SubLvPrice = calculated_gb;
                }
                else if(IsOZStructureValid3(ozNumber.Trim()))
                {
                    if (lv_amount != "0,00")
                    {
                        InsertHeaderRow(pdfDocument, ozNumber, short_text, i);
                        InsertNormalDataRow(pdfDocument, lv_amount, lv_amount_unit, calculated_gb.ToString("0.00"), ep_basic, i);
                        if (!(string.IsNullOrWhiteSpace(long_text)))
                        {
                            InsertLongDataRow(pdfDocument, long_text, i);
                        }
                    }
                    else
                    {
                        
                    }
                }
                
                
                // Ausgabe letzter TotalSum
                if (i + 1 == dataLVsFromDatabase.Count)
                {
                    if (LvPrice != 0.00)
                    {
                        innerList.Add(new Tuple<string, string, double>(CurrentSubNumber.ToString(), CurrentSubShort_Text, SubLvPrice));
                        innerList.Add(new Tuple<string, string, double>(CurrentMainNumber.ToString(), CurrentMainShort_Text, LvPrice));
                        InsertSubSumRow(pdfDocument, CurrentSubNumber, CurrentSubShort_Text, 0, i);
                        InsertSumRow(pdfDocument, CurrentMainNumber, CurrentMainShort_Text, 0, i);
                        overviewList.Add(innerList);
                    }
                }
                
            }

            InsertOverviewPage(pdfDocument);

            InsertSignaturePage(pdfDocument);
            
            InsertFooterHeaderData(pdfDocument);
            
            InsertStaticData(pdfDocument, porposalData);
            
            pdfDocument.Close();
            
            PrintOverviewList();
        }
    }
    
    
    public static void InsertFooterHeaderData(PdfDocument pdfDocument)
    {
        int numberOfPages = pdfDocument.GetNumberOfPages();
        for (int i = 1; i <= numberOfPages; i++)
        {
            if (i == 1)
            {
                PdfPage page = pdfDocument.GetPage(i);
                PdfCanvas canvas = new PdfCanvas(page);
                Rectangle pageSize = page.GetPageSize();
                
                InsertFooterData(pdfDocument, 40, 62, 516, i);
            }
            else
            {
                if (i == pdfDocument.GetNumberOfPages())
                {
                    PdfPage page = pdfDocument.GetPage(i);
                    PdfCanvas canvas = new PdfCanvas(page);
                    Rectangle pageSize = page.GetPageSize();
                    
                    InsertFooterData(pdfDocument, 40, 62, 516, i);
                    CreateSpecificFormPageCnt(136, 712, 50, 10, pdfDocument, $"{proposalId}{i}", proposalId, 7.92f, i);
                    CreateSpecificFormPageCnt(381, 712, 50, 10, pdfDocument, $"{LvName}{i+1}", LvName, 7.92f, i);
                    CreateSpecificFormPageCnt(381, 702, 50, 10, pdfDocument, $"{LvName}{i+2}", LvName, 7.92f, i);
                    CreateSpecificFormPageCnt(381, 692, 50, 10, pdfDocument, $"{LvName}{i+3}", LvName, 7.92f, i);
                    CreateSpecificFormPageCnt(57, 600, 600, 10, pdfDocument, $"LcVontains", $"Das LV besteht aus den Seiten 1 bis {pdfDocument.GetNumberOfPages()}", 10.08f, i);
                    CreateSpecificFormPageCnt(57, 538, 80, 10, pdfDocument, $"Rastenfeld{i}", $"Rastenfeld", 12, i);
                    CreateSpecificFormPageCnt(225, 538, 80, 10, pdfDocument, $"Date{i}", $"{DateTime.UtcNow.ToLocalTime().Date:dd.MM.yyyy}", 12, i);
                    
                }
                else if(i == pdfDocument.GetNumberOfPages() - 1)
                {
                    InsertFooterData(pdfDocument, 40, 62, 516, i);
                    CreateSpecificFormPageCnt(136, 710, 50, 10, pdfDocument, $"{proposalId}{i}", proposalId, 7.92f, i);
                    CreateSpecificFormPageCnt(381, 710, 50, 10, pdfDocument, $"{LvName}{i+1}", LvName, 7.92f, i);
                    CreateSpecificFormPageCnt(381, 700, 50, 10, pdfDocument, $"{LvName}{i+2}", LvName, 7.92f, i);
                    CreateSpecificFormPageCnt(381, 690, 50, 10, pdfDocument, $"{LvName}{i+3}", LvName, 7.92f, i);
                    //CreateSpecificFormPageCnt(57, 536, 80, 10, pdfDocument, $"Rastenfeld{i}", "Rastenfeld", 12, i);
                }
                else
                {
                    PdfPage page = pdfDocument.GetPage(i);
                    PdfCanvas canvas = new PdfCanvas(page);
                    Rectangle pageSize = page.GetPageSize();
                    
                    InsertFooterData(pdfDocument, 40, 62, 516, i);
                    CreateSpecificFormPageCnt(136, 724, 50, 10, pdfDocument, $"{proposalId}{i}", proposalId, 7.92f, i);
                    CreateSpecificFormPageCnt(381, 724, 50, 10, pdfDocument, $"{LvName}{i+1}", LvName, 7.92f, i);
                    CreateSpecificFormPageCnt(381, 714, 50, 10, pdfDocument, $"{LvName}{i+2}", LvName, 7.92f, i);
                    CreateSpecificFormPageCnt(381, 704, 50, 10, pdfDocument, $"{LvName}{i+3}", LvName, 7.92f, i);
                    
                }
            }
            
        }
    }
    
    public static void PrintOverviewList()
    {
        foreach (var outerList in overviewList)
        {
            Console.WriteLine("Outer List:");
            foreach (var tuple in outerList)
            {
                Console.WriteLine($"    Tuple: {tuple.Item1}, {tuple.Item2}, {tuple.Item3}");
            }
        }
    }


    public static void InsertSignaturePage(PdfDocument pdfDocument)
    {
        PdfDocument template = new PdfDocument(new PdfReader("Kleinangebote_LV_Signature_Page.pdf"));
        pdfDocument.AddPage(template.GetFirstPage().CopyTo(pdfDocument));
        yPos = 610;
        PageCnt++;
    }

    public static void InsertFooterData(PdfDocument pdfDocument, int yPos, int xPosDate, int xPosPage, int i)
    {
        CreateSpecificFormPageCnt(xPosDate, yPos, 100, 10, pdfDocument, $"FooterDateTime{i}", $"{DateTime.UtcNow.ToLocalTime().Date:dd.MM.yyyy}   {DateTime.UtcNow.ToLocalTime().TimeOfDay:hh\\:mm}",6, i);
        CreateSpecificFormFieldBoldPageCnt(xPosPage, yPos, 50, 10, pdfDocument, $"PageCntFooter{i}", $"Seite {i} von {pdfDocument.GetNumberOfPages()}", 6, i);
    }

    public static void InsertOverviewPage(PdfDocument pdfDocument)
    {
        pdfDocument.RemovePage(PageCnt);
        PdfDocument template = new PdfDocument(new PdfReader("Kleinangebote_LV_Overview_Page.pdf"));
        pdfDocument.AddPage(template.GetFirstPage().CopyTo(pdfDocument));
        //InsertFooterData(pdfDocument, 41, 62, 516);
        PageCnt++;
        yPos = 600;
        int i = 1;
        int cnt = 0;
        foreach (var outerList in overviewList)
        {
            int itemCnt = outerList.Count;
            foreach (var tuple in outerList)
            {
                CreateSpecificFormField(55, yPos, 200, 15, pdfDocument, $"SubOverviewOz{cnt}", $"{tuple.Item1}", 10.08f);
                CreateSpecificFormField(226, yPos, 350, 15, pdfDocument, $"SubOverviewshort_text{cnt}", $"{tuple.Item2}", 10.08f);
                
                if (i != itemCnt)
                {
                    CreateSpecificFormField(461, yPos, 80, 15, pdfDocument, $"SubOverviewSum{cnt}", $"{tuple.Item3.ToString("0.00")}", 10.08f);
                }
                else
                {
                    CreateSpecificFormFieldBold(548, yPos, 80, 15, pdfDocument, $"SubOverviewSum{cnt}", $"{tuple.Item3.ToString("0.00")}", 10.08f);

                }
                
                if (i == itemCnt-1)
                {
                    yPos -= 1;
                    PdfCanvas canvas = new PdfCanvas(pdfDocument.GetPage(PageCnt-1));
                    canvas.MoveTo(55, yPos);
                    canvas.LineTo(572, yPos);
                    canvas.ClosePathStroke();

                    yPos -= 5;
                }
                
                cnt++;
                i++;
                yPos -= 11;
            }

            i = 1;
            yPos -= 12;
        }
        yPos -= 10;
        
        //Create Total Price Section
        PdfCanvas canvasTotal = new PdfCanvas(pdfDocument.GetPage(PageCnt-1));
        canvasTotal.MoveTo(55, yPos);
        canvasTotal.LineTo(572, yPos);
        canvasTotal.ClosePathStroke();
        yPos -= 20;
        
        CreateSpecificFormFieldBold(55, yPos, 200, 15, pdfDocument, $"TotalLVSumDes{cnt}", "LV", 10.08f);
        CreateSpecificFormFieldBold(548, yPos, 80, 15, pdfDocument, $"TotalLVSum{cnt}", (TotalPriceOfLv+Convert.ToDouble(DiscountValue)).ToString("0.00"), 10.08f);
        
        yPos -= 15;
        //Discount
        if (Convert.ToDouble(DiscountValue) > 0)
        {
            CreateSpecificFormFieldBold(55, yPos, 300, 15, pdfDocument, "discount", "Preisnachlass", 10.08f);
            CreateSpecificFormFieldBold(548, yPos, 300, 15, pdfDocument, "overviewdiscountValue", (DiscountValue), 10.08f);   
        }

        yPos -= 15;
        
        canvasTotal.MoveTo(127, yPos);
        canvasTotal.LineTo(572, yPos);
        canvasTotal.ClosePathStroke();
        
        yPos -= 20;
        
        CreateSpecificFormField(127, yPos, 300, 15, pdfDocument, "overviewTotalPriceEuro", "Gesamtpreis in EUR", 10.08f);
        CreateSpecificFormField(504, yPos, 300, 15, pdfDocument, "overviewTotalPriceValue", (TotalPriceOfLv).ToString("0.00"), 10.08f);
        CreateSpecificFormField(547, yPos, 300, 15, pdfDocument, "overviewTotalPriceUnit", "EUR", 10.08f);
        

        yPos -= 12;
        
        CreateSpecificFormField(127, yPos, 300, 15, pdfDocument, "overviewMwst", "Zuzüglich der gestzlichen Umsatzsteuer von 20,00 %", 10.08f);
        CreateSpecificFormField(504, yPos, 300, 15, pdfDocument, "overviewMwstTotalPriceValue", ((TotalPriceOfLv)*0.20).ToString("0.00"), 10.08f);
        CreateSpecificFormField(547, yPos, 300, 15, pdfDocument, "overviewMwstTotalPriceUnit", "EUR", 10.08f);
        
        yPos -= 5;
        
        canvasTotal.MoveTo(127, yPos);
        canvasTotal.LineTo(572, yPos);
        canvasTotal.ClosePathStroke();
        
        yPos -= 20;
        
        CreateSpecificFormFieldBold(127, yPos, 300, 15, pdfDocument, "overviewTextLastRow", "Angebotspreis (zivilrechtlicher Preis) in EUR", 10.08f);
        CreateSpecificFormFieldBold(504, yPos,300, 15, pdfDocument, "overviewTotalPriceOfLV", ((TotalPriceOfLv)*1.20).ToString("0.00"), 10.08f);
        CreateSpecificFormFieldBold(547, yPos, 300, 15, pdfDocument, "overviewTotalPriceOfLVUnit", "EUR", 10.08f);
    }
    
    public static void CreateSpecificFormField(int xPos, int yposition, int width, int height, PdfDocument pdfDocument, string formFieldName, object value, float fontSize)
    {
        Rectangle specificRect = new Rectangle(xPos, yposition, width, height);
        
        PdfTextFormField textFormFieldSpecific =
            new TextFormFieldBuilder(pdfDocument, formFieldName).SetWidgetRectangle(specificRect).CreateText();
        textFormFieldSpecific.SetValue(value.ToString()).SetReadOnly(true);
        textFormFieldSpecific.SetFontAndSize(GetfontArial(), fontSize);
        textFormFieldSpecific.SetMultiline(false);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormFieldSpecific);
    }

    public static void CreateSpecificFormPageCnt(int xPos, int yposition, int width, int height, PdfDocument pdfDocument, string formFieldName, object value, float fontSize, int i)
    {
        PdfPage page = pdfDocument.GetPage(i);
        Rectangle specificRect = new Rectangle(xPos, yposition, width, height);
        PdfTextFormField textFormFieldSpecific =
            new TextFormFieldBuilder(pdfDocument, formFieldName).SetWidgetRectangle(specificRect).CreateText();
        textFormFieldSpecific.SetValue(value.ToString()).SetReadOnly(true);
        textFormFieldSpecific.SetFontAndSize(GetfontArial(), fontSize);
        textFormFieldSpecific.SetMultiline(false);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormFieldSpecific, page);
    }
    
    public static void CreateSpecificFormFieldBold(int xPos, int yposition, int width, int height, PdfDocument pdfDocument, string formFieldName, object value, float fonSize)
    {
        Rectangle specificRect = new Rectangle(xPos, yposition, width, height);
        PdfTextFormField textFormFieldSpecific =
            new TextFormFieldBuilder(pdfDocument, formFieldName).SetWidgetRectangle(specificRect).CreateText();
        textFormFieldSpecific.SetValue(value.ToString()).SetReadOnly(true);
        textFormFieldSpecific.SetFontAndSize(GetfontArialBold(), fonSize);
        textFormFieldSpecific.SetMultiline(false);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormFieldSpecific);
    }
    
    public static void CreateSpecificFormFieldBoldPageCnt(int xPos, int yposition, int width, int height, PdfDocument pdfDocument, string formFieldName, object value, float fonSize, int i)
    {
        PdfPage page = pdfDocument.GetPage(i);
        Rectangle specificRect = new Rectangle(xPos, yposition, width, height);
        PdfTextFormField textFormFieldSpecific =
            new TextFormFieldBuilder(pdfDocument, formFieldName).SetWidgetRectangle(specificRect).CreateText();
        textFormFieldSpecific.SetValue(value.ToString()).SetReadOnly(true);
        textFormFieldSpecific.SetFontAndSize(GetfontArialBold(), fonSize);
        textFormFieldSpecific.SetMultiline(false);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormFieldSpecific, page);
    }

    public static void InsertLongDataRow(PdfDocument pdfDocument, string long_text, int i)
    {
        if (yPos < 105)
        {
            PdfDocument template = new PdfDocument(new PdfReader("Kleinangebote_LV_Vorlage_Form_Empty_Page.pdf"));
            pdfDocument.AddPage(template.GetFirstPage().CopyTo(pdfDocument));
            yPos = 602;
            PageCnt++;
        }
        
        yPos += 10;
        Rectangle rect_long_text = new Rectangle(155, yPos-40, 320, 50);
        PdfTextFormField textFormFieldlong_text = new TextFormFieldBuilder(pdfDocument, $"long_text{i}")
            .SetWidgetRectangle(rect_long_text)
            .CreateText();
        textFormFieldlong_text.SetMultiline(true);
        textFormFieldlong_text.SetValue(long_text).SetReadOnly(true);
        textFormFieldlong_text.SetFontAndSize(GetfontArial(), 10.08f);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormFieldlong_text);
        
        yPos -= 60;
    }

    public static void InsertNormalDataRow(PdfDocument pdfDocument, string lv_amount, string lv_amount_unit, string basic_gb, string ep_basic, int i)
    {
        CreateSpecificFormField(136, yPos, 45, 15, pdfDocument, $"amount_lv{i}", $"ca. {lv_amount}", 10.08f);
        CreateSpecificFormField(178, yPos, 25, 15, pdfDocument, $"lv_amount_unit{i}", lv_amount_unit, 10.08f);
        CreateSpecificFormField(226, yPos, 90, 15, pdfDocument, $"Einheitspreis{i}", $"Einheitspreis:", 10.08f);
        CreateSpecificFormField(549, yPos, 35, 15, pdfDocument, $"basic_gb{i}", basic_gb, 10.08f);
        CreateSpecificFormField(369, yPos, 35, 15, pdfDocument, $"ep_basic{i}", ep_basic, 10.08f);
        
        yPos -= 30;
    }

    public static void InsertSubSumRow(PdfDocument pdfDocument, string ozNumber, string short_text, int sum, int i)
    {
        if (yPos < 105)
        {
            PdfDocument template = new PdfDocument(new PdfReader("Kleinangebote_LV_Vorlage_Form_Empty_Page.pdf"));
            pdfDocument.AddPage(template.GetFirstPage().CopyTo(pdfDocument));
            yPos = 602;
            PageCnt++;
        }
        
        yPos += 10;
        PdfCanvas canvas = new PdfCanvas(pdfDocument.GetPage(PageCnt));
        canvas.MoveTo(129, yPos);
        canvas.LineTo(572, yPos);
        canvas.ClosePathStroke();
        
        yPos -= 18;
        
        CreateSpecificFormFieldBold(127, yPos, 200, 15, pdfDocument, $"SubSumOz{i}", $"Summe {ozNumber}", 10.08f);
        CreateSpecificFormFieldBold(226, yPos, 550, 15, pdfDocument, $"SubSumshort_text{i}", $"{short_text}", 10.08f);
        CreateSpecificFormFieldBold(549, yPos, 80, 15, pdfDocument, $"SubTotalSum{i}", $"{SubLvPrice.ToString("0.00")}", 10.08f);
        
        yPos -= 45;
    }
    
    public static void InsertSumRow(PdfDocument pdfDocument, string ozNumber, string short_text, int sum, int i)
    {
        if (yPos < 105)
        {
            PdfDocument template2 = new PdfDocument(new PdfReader("Kleinangebote_LV_Vorlage_Form_Empty_Page.pdf"));
            pdfDocument.AddPage(template2.GetFirstPage().CopyTo(pdfDocument));
            yPos = 602;
            PageCnt++;
        }
        
        //Draw Line
        yPos -= 30;
        PdfCanvas canvas = new PdfCanvas(pdfDocument.GetPage(PageCnt));
        canvas.MoveTo(129, yPos);
        canvas.LineTo(572, yPos);
        canvas.ClosePathStroke();
        
        yPos -= 18;
        
        CreateSpecificFormFieldBold(127, yPos, 200, 15, pdfDocument, $"TotalSumOz{i}", $"Summe {ozNumber}", 10.08f);
        CreateSpecificFormFieldBold(226, yPos, 550, 15, pdfDocument, $"TotalSumshort_text{i}", $"{short_text}", 10.08f);
        CreateSpecificFormFieldBold(549, yPos, 80, 15, pdfDocument, $"TotalSum{i}", $"{LvPrice.ToString("0.00")}", 10.08f);

        
        PdfDocument template = new PdfDocument(new PdfReader("Kleinangebote_LV_Vorlage_Form_Empty_Page.pdf"));
        pdfDocument.AddPage(template.GetFirstPage().CopyTo(pdfDocument));
        yPos = 602;
        PageCnt++;
    }

    public static void InsertHeaderRow(PdfDocument pdfDocument, string ozNumber, string short_text, int i)
    {
        if (yPos < 105)
        {
            PdfDocument template = new PdfDocument(new PdfReader("Kleinangebote_LV_Vorlage_Form_Empty_Page.pdf"));
            pdfDocument.AddPage(template.GetFirstPage().CopyTo(pdfDocument));
            yPos = 602;
            
            PageCnt++;
        }
        
        CreateSpecificFormFieldBold(56, yPos, 45, 15, pdfDocument, $"OZNumber{i}", ozNumber, 10.08f);
        Console.WriteLine(short_text);
        CreateSpecificFormFieldBold(127, yPos, 500, 15, pdfDocument, $"short_text{i}", short_text, 10.08f);
        CreateSpecificFormFieldBold(453, yPos, 39, 15, pdfDocument, $"Letter{i}", "Z", 10.08f);

        yPos -= 30;
    }

    //Check Structure of OZ Number
    public static bool IsOZStructureValid3(string ozNumber)
    {
        return Regex.IsMatch(ozNumber, @"^\d{2}\.\d{2}\.\d{2}\.$");
    }
    
    public static bool IsOZStructureValid2(string ozNumber)
    {
        return Regex.IsMatch(ozNumber, @"^\d{2}\.\d{2}\.$");
    }
    
    public static bool IsOZStructureValid1(string ozNumber)
    {
        return Regex.IsMatch(ozNumber, @"^\d{2}\.$");
    }
    
    
    // Insert Static Data Method
    public static void InsertStaticData(PdfDocument pdfDocument, List<Dictionary<string, object>> dataFromDatabase)
    {
        
        string date = (dataFromDatabase.First().TryGetValue("created_at", out object createdAtObject) && createdAtObject != null) ? Convert.ToDateTime(createdAtObject).ToString("dd.MM.yyyy") : null;
        string proposalId = dataFromDatabase.First().TryGetValue("proposal_id", out object proposalIdObject) ? Convert.ToInt32(proposalIdObject).ToString() : null;
        string projectName = dataFromDatabase.First().TryGetValue("project_name", out object projectNameObject) ? Convert.ToString(projectNameObject).ToString() : null;
        
        UpdateFormField(pdfDocument, "Ort Datum", "Rastenfeld,   " + date, 12, GetfontArial());
        UpdateFormField(pdfDocument, "DateOfOffer", date, 10, GetfontArial());
        UpdateFormField(pdfDocument, "OfferNumber", proposalId, 10, GetfontArialBold());
        UpdateFormField(pdfDocument, "ProjectName", projectName, 10, GetfontArialBold());

        // Durch Datenbank Werte ersetzen
        UpdateFormField(pdfDocument, "TotalPrice", (TotalPriceOfLv).ToString("0.00"), 12, GetfontArial());
        UpdateFormField(pdfDocument, "taxAmount", ((TotalPriceOfLv)*0.20).ToString("0.00"), 12, GetfontArial());
        UpdateFormField(pdfDocument, "offerPrice", ((TotalPriceOfLv)*1.20).ToString("0.00"), 12, GetfontArial());
        //UpdateFormField(pdfDocument, "Ort", "Rastenfeld", 10, GetfontArial());
        //UpdateFormField(pdfDocument, "Datum", date, 10, GetfontArial());
        PageCnt++;
    }
    
    
    // Update existing form fields
    public static void UpdateFormField(PdfDocument pdfDocument, string fieldName, string value, float fontSize, PdfFont font)
    {
        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDocument, true);

        if (form.GetField(fieldName) != null)
        {
            form.GetField(fieldName).SetValue(value);


            PdfFont fontArial = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            form.GetField(fieldName).SetFontAndSize(font, fontSize);

            form.GetField(fieldName).SetReadOnly(true);
        }
        else
        {
            Console.WriteLine($"Formularfeld mit dem Namen '{fieldName}' nicht gefunden.");
        }
    }

    
    //Create Font Methoden
    public static PdfFont GetfontArialBold()
    {
        PdfFont fontArialBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        return fontArialBold;
    }
    
    public static PdfFont GetfontArial()
    {
        PdfFont fontArial = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        return fontArial;
    }
}