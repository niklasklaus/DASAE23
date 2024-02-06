using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using iText.Kernel.Pdf;
using iText.Forms;
using iText.Forms.Exceptions;
using iText.Forms.Fields;
using iText.Forms.Logs;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.StyledXmlParser.Jsoup.Select;
using iTextSharp.text;
using MySqlConnector;
using Rectangle = iText.Kernel.Geom.Rectangle;

class Program
{
    static void Main()
    {
        PdfFont fontArialBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        PdfFont fontArial = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        
        // Database Connection
        string connectionString = "Server=localhost;Database=da_dbschema;User Id=root;Password=root;";

        string workingDirectory = Environment.CurrentDirectory;
        Console.WriteLine("Current working directory: " + workingDirectory);
        
        // PDF Pfade
        string sourcePdfPath = $"Kleinangebote_LV_Vorlage_Form_Delete.pdf";
        string destPdfPath = $"pdfs/modified_pdf.pdf";
        //string headerPdfPath = @"E:\Diplomarbeit\PDFGenerierung_iText\Kleinangebote_LV_Vorlage_Form_Copy.pdf";

        //string sqlStatement = "SELECT * FROM PROPOSALS WHERE proposal_id = 2677215";
        string sqlStatement = "SELECT * FROM PROPOSALS WHERE proposal_id = 844898";
        
        // Retrieve data from the database
        List<Dictionary<string, object>> dataFromDatabase = RetrieveDataListFromDatabase(connectionString, sqlStatement);
        string date = (dataFromDatabase.First().TryGetValue("created_at", out object createdAtObject) && createdAtObject != null)
                        ? Convert.ToDateTime(createdAtObject).ToString("dd.MM.yyyy")
                        : null;

        string proposalId = dataFromDatabase.First().TryGetValue("proposal_id", out object proposalIdObject) ? Convert.ToInt32(proposalIdObject).ToString() : null;
        string projectName = dataFromDatabase.First().TryGetValue("project_name", out object projectNameObject) ? Convert.ToString(projectNameObject).ToString() : null;

        using (PdfDocument pdfDocument = new PdfDocument(new PdfReader(sourcePdfPath), new PdfWriter(destPdfPath)))
        {
            UpdateFormField(pdfDocument, "DateOfOffer", date, 10, fontArial);
            UpdateFormField(pdfDocument, "OfferNumber", proposalId, 10, fontArialBold);
            UpdateFormField(pdfDocument, "ProjectName", projectName, 10, fontArialBold);

            // Durch Datenbank Werte ersetzen
            UpdateFormField(pdfDocument, "TotalPrice", 0.ToString("0.00"), 12, fontArial);
            UpdateFormField(pdfDocument, "taxAmount", (0).ToString("0.00"), 12, fontArial);
            UpdateFormField(pdfDocument, "offerPrice", (0).ToString("0.00"), 12, fontArial);

            string sqlStatementLvs = "SELECT * FROM LVS WHERE proposal_id = 844898";
            List<Dictionary<string, object>> dataLVsFromDatabase = RetrieveDataListFromDatabase(connectionString, sqlStatementLvs);

            // Position Body
            CreateFormPositionBody(pdfDocument, dataLVsFromDatabase);

            pdfDocument.Close();
        }
    }

    static void CreateFormPositionBody(PdfDocument pdfDocument, List<Dictionary<string, object>> dataLVsFromDatabase)
    {
        double fixSubPrice = 0.0;
        double fixPrice = 0.0;
        
        double subPositionSum = 0.0;
        double positionSum = 0.0;
        
        int pageCnt = 2;
        int yPos = 546;
        string currentTotalSumOzNumber = "";
        string currentTotalSumOzShort_text = "";
        string currentSumOzNumber = "";
        string currentSumOzShort_text = "";
        //int i = 0;

        PdfFont fontArialBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        float fontSizeStandard = 10;
        
        
        for (int i = 0; i<dataLVsFromDatabase.Count-1; i++)
        {
            string ozNumberNext =  (dataLVsFromDatabase[i+1].TryGetValue("oz", out object ozNumberNextObject) && ozNumberNextObject != null)
                ? Convert.ToString(ozNumberNextObject) : null;
            
            string ozNumber = (dataLVsFromDatabase[i].TryGetValue("oz", out object ozNumberObject) && ozNumberObject != null)
                ? Convert.ToString(ozNumberObject) : null;
            string short_text = (dataLVsFromDatabase[i].TryGetValue("short_text", out object short_textObject) && short_textObject != null)
                ? Convert.ToString(short_textObject) : null;

            Console.WriteLine(short_text);
            
            if (IsOZStructureValid1(ozNumber))
            {
                currentTotalSumOzNumber = ozNumber;
                currentTotalSumOzShort_text = short_text;
                
                fixSubPrice = subPositionSum;
                subPositionSum = 0.0;
                
                positionSum += fixSubPrice;
                fixPrice = positionSum;
                positionSum = 0.0;
            }
            if (IsOZStructureValid2(ozNumber))
            {
                currentSumOzNumber = ozNumber;
                currentSumOzShort_text = short_text;
                
                fixSubPrice = subPositionSum;
                subPositionSum = 0.0;
                
                positionSum += fixSubPrice;
            }
            
            if (yPos < 105) {
                PdfDocument template = new PdfDocument(new PdfReader("Kleinangebote_LV_Vorlage_Form_Empty_Page.pdf"));
                //pdfDocument.AddNewPage();
                pdfDocument.AddPage(template.GetFirstPage().CopyTo(pdfDocument));
                yPos = 602;
                pageCnt++;
            }

            Rectangle rect = new Rectangle(56, yPos, 45, 15);

            PdfTextFormField textFormField =
                new TextFormFieldBuilder(pdfDocument, $"OZNumber{i}").SetWidgetRectangle(rect).CreateText();
            textFormField.SetValue(ozNumber).SetReadOnly(true);
            textFormField.SetFontAndSize(fontArialBold, fontSizeStandard);
            textFormField.SetMultiline(false);
            PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormField);

            Console.WriteLine(short_text);
            
            Rectangle rect2 = new Rectangle(127, yPos, 200, 15);

            PdfTextFormField textFormField2 =
                new TextFormFieldBuilder(pdfDocument, $"short_text{i}").SetWidgetRectangle(rect2).CreateText();
            textFormField2.SetValue(short_text).SetReadOnly(true);
            textFormField2.SetFontAndSize(fontArialBold, fontSizeStandard);
            textFormField2.SetMultiline(false);
            PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormField2);
            
            
            Rectangle rect3 = new Rectangle(453, yPos, 39, 15);

            PdfTextFormField textFormField3 =
                new TextFormFieldBuilder(pdfDocument, $"Letter{i}").SetWidgetRectangle(rect3).CreateText();
            textFormField3.SetValue("Z").SetReadOnly(true);
            textFormField3.SetFontAndSize(fontArialBold, fontSizeStandard);
            textFormField3.SetMultiline(false);
            PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormField3);
            
            if (IsOZStructureValid3(ozNumber.Trim()))
            {
                string test = (dataLVsFromDatabase[i].TryGetValue("long_text", out object long_textObject) && long_textObject != null)
                    ? Convert.ToString(long_textObject) : null;
                
                if (string.IsNullOrWhiteSpace(test))
                {
                    if (IsOZStructureValid2(ozNumberNext.Trim()))
                    {
                        if (yPos < 105)
                        {
                            PdfDocument template = new PdfDocument(new PdfReader(
                                "Kleinangebote_LV_Vorlage_Form_Empty_Page.pdf"));
                            //pdfDocument.AddNewPage();
                            pdfDocument.AddPage(template.GetFirstPage().CopyTo(pdfDocument));
                            yPos = 602;
                            pageCnt++;
                        }

                        subPositionSum += CreatePositionDataRow(yPos, dataLVsFromDatabase[i], pdfDocument, i);
                        CreateSumPositionDataRow(yPos, pdfDocument, i, pageCnt, currentSumOzNumber,
                            currentSumOzShort_text, subPositionSum);
                        yPos -= 93;
                    }
                    else if (IsOZStructureValid1(ozNumberNext.Trim()))
                    {
                        PdfDocument template = new PdfDocument(new PdfReader(
                            "Kleinangebote_LV_Vorlage_Form_Empty_Page.pdf"));


                        if (yPos < 150)
                        {
                            //pdfDocument.AddNewPage();
                            pdfDocument.AddPage(template.GetFirstPage().CopyTo(pdfDocument));
                            yPos = 602;
                            pageCnt++;
                        }

                        subPositionSum += CreatePositionDataRow(yPos, dataLVsFromDatabase[i], pdfDocument, i);
                        CreateSumPositionDataRow(yPos, pdfDocument, i, pageCnt, currentSumOzNumber,
                            currentSumOzShort_text, subPositionSum);
                        CreateTotalSumPositionDataRow(yPos, pdfDocument, i, pageCnt, currentTotalSumOzNumber,
                            currentTotalSumOzShort_text, fixPrice);
                        //pdfDocument.AddNewPage();
                        pdfDocument.AddPage(template.GetFirstPage().CopyTo(pdfDocument));
                        yPos = 602;
                        pageCnt++;
                    }
                    else
                    {
                        if (yPos < 105)
                        {
                            PdfDocument template = new PdfDocument(new PdfReader(
                                "Kleinangebote_LV_Vorlage_Form_Empty_Page.pdf"));
                            //pdfDocument.AddNewPage();
                            pdfDocument.AddPage(template.GetFirstPage().CopyTo(pdfDocument));
                            yPos = 602;
                            pageCnt++;
                        }

                        subPositionSum += CreatePositionDataRow(yPos, dataLVsFromDatabase[i], pdfDocument, i);
                        yPos -= 58;
                    }
                }
                else
                {
                    yPos = CreateLongTextRow(yPos, dataLVsFromDatabase[i], pdfDocument, i);
                    
                    
                    
                    if (IsOZStructureValid2(ozNumberNext.Trim()))
                    {
                        if (yPos < 105)
                        {
                            PdfDocument template = new PdfDocument(new PdfReader(
                                "Kleinangebote_LV_Vorlage_Form_Empty_Page.pdf"));
                            //pdfDocument.AddNewPage();
                            pdfDocument.AddPage(template.GetFirstPage().CopyTo(pdfDocument));
                            yPos = 602;
                            pageCnt++;
                        }

                        subPositionSum += CreatePositionDataRow(yPos, dataLVsFromDatabase[i], pdfDocument, i);
                        CreateSumPositionDataRow(yPos, pdfDocument, i, pageCnt, currentSumOzNumber,
                            currentSumOzShort_text, fixSubPrice);
                        yPos -= 93;
                    }
                    else if (IsOZStructureValid1(ozNumberNext.Trim()))
                    {
                        PdfDocument template = new PdfDocument(new PdfReader(
                            "Kleinangebote_LV_Vorlage_Form_Empty_Page.pdf"));


                        if (yPos < 150)
                        {
                            //pdfDocument.AddNewPage();
                            pdfDocument.AddPage(template.GetFirstPage().CopyTo(pdfDocument));
                            yPos = 602;
                            pageCnt++;
                        }

                        subPositionSum += CreatePositionDataRow(yPos, dataLVsFromDatabase[i], pdfDocument, i);
                        CreateSumPositionDataRow(yPos, pdfDocument, i, pageCnt, currentSumOzNumber,
                            currentSumOzShort_text, fixSubPrice);
                        CreateTotalSumPositionDataRow(yPos, pdfDocument, i, pageCnt, currentTotalSumOzNumber,
                            currentTotalSumOzShort_text, fixPrice);
                        //pdfDocument.AddNewPage();
                        pdfDocument.AddPage(template.GetFirstPage().CopyTo(pdfDocument));
                        yPos = 602;
                        pageCnt++;
                    }
                    else
                    {
                        if (yPos < 105)
                        {
                            PdfDocument template = new PdfDocument(new PdfReader(
                                "Kleinangebote_LV_Vorlage_Form_Empty_Page.pdf"));
                            //pdfDocument.AddNewPage();
                            pdfDocument.AddPage(template.GetFirstPage().CopyTo(pdfDocument));
                            yPos = 602;
                            pageCnt++;
                        }

                        subPositionSum += CreatePositionDataRow(yPos, dataLVsFromDatabase[i], pdfDocument, i);
                        yPos -= 58;
                    }
                }
            }
            else
            {
                yPos -= 24;
            }
        }
        
        //Hier soll das letze Element in der Console ausgegeben werden
        
        int lastIndex = dataLVsFromDatabase.Count - 1;
        
        string ozNumberLast = (dataLVsFromDatabase[lastIndex].TryGetValue("oz", out object ozNumberLastObject) && ozNumberLastObject != null)
            ? Convert.ToString(ozNumberLastObject) : null;
        string short_textLast = (dataLVsFromDatabase[lastIndex].TryGetValue("short_text", out object short_textLastObject) && short_textLastObject != null)
            ? Convert.ToString(short_textLastObject) : null;

        Console.WriteLine("-----------------");
        Console.WriteLine(ozNumberLast);
        Console.WriteLine(short_textLast);
        
        
        Rectangle rectLast = new Rectangle(56, yPos, 50, 15);

        PdfTextFormField textFormFieldLast =
            new TextFormFieldBuilder(pdfDocument, $"OZNumber{lastIndex}").SetWidgetRectangle(rectLast).CreateText();
        textFormFieldLast.SetValue(ozNumberLast).SetReadOnly(true);
        textFormFieldLast.SetFontAndSize(fontArialBold, fontSizeStandard);
        textFormFieldLast.SetMultiline(false);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormFieldLast);

        Console.WriteLine(short_textLast);
            
        Rectangle rect2Last = new Rectangle(127, yPos, 250, 15);

        PdfTextFormField textFormField2Last =
            new TextFormFieldBuilder(pdfDocument, $"short_text{lastIndex}").SetWidgetRectangle(rect2Last).CreateText();
        textFormField2Last.SetValue(short_textLast).SetReadOnly(true);
        textFormField2Last.SetFontAndSize(fontArialBold, fontSizeStandard);
        textFormField2Last.SetMultiline(false);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormField2Last);
            
            
        Rectangle rect3Last = new Rectangle(453, yPos, 39, 15);

        PdfTextFormField textFormField3Last =
            new TextFormFieldBuilder(pdfDocument, $"Letter{lastIndex}").SetWidgetRectangle(rect3Last).CreateText();
        textFormField3Last.SetValue("Z").SetReadOnly(true);
        textFormField3Last.SetFontAndSize(fontArialBold, fontSizeStandard);
        textFormField3Last.SetMultiline(false);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormField3Last);
        
        
        string testLast = (dataLVsFromDatabase[lastIndex].TryGetValue("long_text", out object long_textLastObject) && long_textLastObject != null)
            ? Convert.ToString(long_textLastObject) : null;

        if (string.IsNullOrWhiteSpace(testLast))
        {
            subPositionSum += CreatePositionDataRow(yPos, dataLVsFromDatabase[lastIndex], pdfDocument, lastIndex);
            fixSubPrice = subPositionSum;
            positionSum += fixSubPrice;
            CreateSumPositionDataRow(yPos, pdfDocument, lastIndex, pageCnt, currentSumOzNumber,
                currentSumOzShort_text, fixSubPrice);
            fixPrice = positionSum;
            CreateTotalSumPositionDataRow(yPos, pdfDocument, lastIndex, pageCnt, currentTotalSumOzNumber,
                currentTotalSumOzShort_text, fixPrice);
        }
        else
        {
            yPos = CreateLongTextRow(yPos, dataLVsFromDatabase[lastIndex], pdfDocument, lastIndex);
            subPositionSum += CreatePositionDataRow(yPos, dataLVsFromDatabase[lastIndex], pdfDocument, lastIndex);
            fixSubPrice = subPositionSum;
            positionSum += fixSubPrice;
            CreateSumPositionDataRow(yPos, pdfDocument, lastIndex, pageCnt, currentSumOzNumber,
                currentSumOzShort_text, fixSubPrice);
            fixPrice = positionSum;
            CreateTotalSumPositionDataRow(yPos, pdfDocument, lastIndex, pageCnt, currentTotalSumOzNumber,
                currentTotalSumOzShort_text, fixPrice);
        }
    }

    static void CreateSumPositionDataRow(int yPos, PdfDocument pdfDocument, int i, int pageCnt, string currentSumOzNumber, string currentSumOzShort_text, double subPositionSum)
    {
        PdfFont fontArialBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        float fontSizeStandard = 10;
        
        //Draw Line
        yPos -= 51;
        PdfCanvas canvas = new PdfCanvas(pdfDocument.GetPage(pageCnt));
        canvas.MoveTo(129, yPos);
        canvas.LineTo(572, yPos);
        canvas.ClosePathStroke();

        yPos -= 18;
        Rectangle rectSumOz = new Rectangle(127, yPos, 80, 15);

        PdfTextFormField textFormFieldSumOz =
            new TextFormFieldBuilder(pdfDocument, $"SumOz{i}").SetWidgetRectangle(rectSumOz).CreateText();
        textFormFieldSumOz.SetValue($"Summe {currentSumOzNumber}").SetReadOnly(true);
        textFormFieldSumOz.SetFontAndSize(fontArialBold, fontSizeStandard);
        textFormFieldSumOz.SetMultiline(false);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormFieldSumOz);
        
        
        Rectangle rectSumshort_text = new Rectangle(226, yPos, 200, 15);
        
        PdfTextFormField textFormFieldSumshort_text =
            new TextFormFieldBuilder(pdfDocument, $"Sumshort_text{i}").SetWidgetRectangle(rectSumshort_text).CreateText();
        textFormFieldSumshort_text.SetValue($"{currentSumOzShort_text}").SetReadOnly(true);
        textFormFieldSumshort_text.SetFontAndSize(fontArialBold, fontSizeStandard);
        textFormFieldSumshort_text.SetMultiline(false);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormFieldSumshort_text);
        
        
        Rectangle rectSum = new Rectangle(549, yPos, 80, 15);
        
        PdfTextFormField textFormFieldSum =
            new TextFormFieldBuilder(pdfDocument, $"Sum{i}").SetWidgetRectangle(rectSum).CreateText();
        textFormFieldSum.SetValue($"{subPositionSum}").SetReadOnly(true);
        textFormFieldSum.SetFontAndSize(fontArialBold, fontSizeStandard);
        textFormFieldSum.SetMultiline(false);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormFieldSum);
    }
    
    static void CreateTotalSumPositionDataRow(int yPos, PdfDocument pdfDocument, int i, int pageCnt, string currentSumOzNumber, string currentSumOzShort_text, double fixPrice)
    {
        PdfFont fontArialBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        float fontSizeStandard = 10;
        
        //Draw Line
        yPos -= 90;
        PdfCanvas canvas = new PdfCanvas(pdfDocument.GetPage(pageCnt));
        canvas.MoveTo(129, yPos);
        canvas.LineTo(572, yPos);
        canvas.ClosePathStroke();

        yPos -= 18;
        Rectangle rectSumOz = new Rectangle(127, yPos, 200, 15);

        PdfTextFormField textFormFieldSumOz =
            new TextFormFieldBuilder(pdfDocument, $"TotalSumOz{i}").SetWidgetRectangle(rectSumOz).CreateText();
        textFormFieldSumOz.SetValue($"Summe {currentSumOzNumber}").SetReadOnly(true);
        textFormFieldSumOz.SetFontAndSize(fontArialBold, fontSizeStandard);
        textFormFieldSumOz.SetMultiline(false);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormFieldSumOz);
        
        
        Rectangle rectSumshort_text = new Rectangle(226, yPos, 350, 15);
        
        PdfTextFormField textFormFieldSumshort_text =
            new TextFormFieldBuilder(pdfDocument, $"TotalSumshort_text{i}").SetWidgetRectangle(rectSumshort_text).CreateText();
        textFormFieldSumshort_text.SetValue($"{currentSumOzShort_text}").SetReadOnly(true);
        textFormFieldSumshort_text.SetFontAndSize(fontArialBold, fontSizeStandard);
        textFormFieldSumshort_text.SetMultiline(false);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormFieldSumshort_text);
        
        
        Rectangle rectSum = new Rectangle(549, yPos, 80, 15);
        
        PdfTextFormField textFormFieldSum =
            new TextFormFieldBuilder(pdfDocument, $"TotalSum{i}").SetWidgetRectangle(rectSum).CreateText();
        textFormFieldSum.SetValue($"{fixPrice}").SetReadOnly(true);
        textFormFieldSum.SetFontAndSize(fontArialBold, fontSizeStandard);
        textFormFieldSum.SetMultiline(false);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormFieldSum);
    }

    static double CreatePositionDataRow(int yPos, Dictionary<string, object> dataLVsFromDatabase, PdfDocument pdfDocument, int i)
    {
        PdfFont fontArial = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        float fontSizeStandard = 10;
        
        yPos -= 35;
        string lv_amount = (dataLVsFromDatabase.TryGetValue("lv_amount", out object lv_amountObject) && lv_amountObject != null)
            ? Convert.ToDouble(lv_amountObject).ToString("0.00") : null;
        
        string ep_basic = (dataLVsFromDatabase.TryGetValue("basic_ep", out object ep_basicObject) && ep_basicObject != null)
            ? Convert.ToString(ep_basicObject) : null;
        
        
        string lv_amount_unit = (dataLVsFromDatabase.TryGetValue("lv_amount_unit", out object lv_amount_unitObject) && lv_amount_unitObject != null)
            ? Convert.ToString(lv_amount_unitObject) : null;
        
        string basic_gb = (dataLVsFromDatabase.TryGetValue("calculated_gb", out object basic_gbObject) && basic_gbObject != null)
            ? Convert.ToString(basic_gbObject) : null;
        
        
        Rectangle rect = new Rectangle(136, yPos, 45, 15);
        PdfTextFormField textFormField =
            new TextFormFieldBuilder(pdfDocument, $"amount_lv{i}").SetWidgetRectangle(rect).CreateText();
        textFormField.SetValue($"ca. {lv_amount}").SetReadOnly(true);
        textFormField.SetFontAndSize(fontArial, fontSizeStandard);
        textFormField.SetMultiline(false);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormField);
        
        
        
        Rectangle rect_lv_amount_unit = new Rectangle(178, yPos, 25, 15);
        PdfTextFormField textFormField_lv_amount_unit =
            new TextFormFieldBuilder(pdfDocument, $"lv_amount_unit{i}").SetWidgetRectangle(rect_lv_amount_unit).CreateText();
        textFormField_lv_amount_unit.SetValue(lv_amount_unit).SetReadOnly(true);
        textFormField_lv_amount_unit.SetFontAndSize(fontArial, fontSizeStandard);
        textFormField_lv_amount_unit.SetMultiline(false);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormField_lv_amount_unit);
        
        
        
        Rectangle rect2 = new Rectangle(226, yPos, 90, 15);
        PdfTextFormField textFormField2 =
            new TextFormFieldBuilder(pdfDocument, $"Einheitspreis{i}").SetWidgetRectangle(rect2).CreateText();
        textFormField2.SetValue($"Einheitspreis:").SetReadOnly(true);
        textFormField2.SetFontAndSize(fontArial, fontSizeStandard);
        textFormField2.SetMultiline(false);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormField2);
        
        
        
        Rectangle rectbasic_gb = new Rectangle(549, yPos, 35, 15);
        PdfTextFormField textFormFieldbasic_gb =
            new TextFormFieldBuilder(pdfDocument, $"basic_gb{i}").SetWidgetRectangle(rectbasic_gb).CreateText();
        textFormFieldbasic_gb.SetValue(basic_gb).SetReadOnly(true);
        textFormFieldbasic_gb.SetFontAndSize(fontArial, fontSizeStandard);
        textFormFieldbasic_gb.SetMultiline(false);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormFieldbasic_gb);
        
        
        
        Rectangle rectep_basic = new Rectangle(369, yPos, 35, 15);
        PdfTextFormField textFormFieldep_basic =
            new TextFormFieldBuilder(pdfDocument, $"ep_basic{i}").SetWidgetRectangle(rectep_basic).CreateText();
        textFormFieldep_basic.SetValue(ep_basic).SetReadOnly(true);
        textFormFieldep_basic.SetFontAndSize(fontArial, fontSizeStandard);
        Console.WriteLine($"Preis: {ep_basic}");
        textFormFieldep_basic.SetMultiline(false);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormFieldep_basic);

        return Convert.ToDouble(basic_gb);
    }

    static int CreateLongTextRow(int yPos, Dictionary<string, object> dataLVsFromDatabase, PdfDocument pdfDocument, int i)
    {
        PdfFont fontArial = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        float fontSizeStandard = 10;
        
        string long_text = (dataLVsFromDatabase.TryGetValue("long_text", out object long_textObject) && long_textObject != null)
            ? Convert.ToString(long_textObject).Replace("\n", "\n") : null;

        yPos -= 12;

        Console.WriteLine("Test: " + long_text);
        
        Rectangle rect_long_text = new Rectangle(155, yPos-40, 320, 50);
        PdfTextFormField textFormFieldlong_text = new TextFormFieldBuilder(pdfDocument, $"long_text{i}")
            .SetWidgetRectangle(rect_long_text)
            .CreateText();
        textFormFieldlong_text.SetMultiline(true);
        textFormFieldlong_text.SetValue(long_text).SetReadOnly(true);
        textFormFieldlong_text.SetFontAndSize(fontArial, fontSizeStandard);
        PdfAcroForm.GetAcroForm(pdfDocument, true).AddField(textFormFieldlong_text);

        yPos -= 40;
        return yPos;
    }
    
    static bool IsOZStructureValid3(string ozNumber)
    {
        return Regex.IsMatch(ozNumber, @"^\d{2}\.\d{2}\.\d{2}\.$");
    }
    
    static bool IsOZStructureValid2(string ozNumber)
    {
        return Regex.IsMatch(ozNumber, @"^\d{2}\.\d{2}\.$");
    }
    
    static bool IsOZStructureValid1(string ozNumber)
    {
        return Regex.IsMatch(ozNumber, @"^\d{2}\.$");
    }
    
    static List<Dictionary<string, object>> RetrieveDataListFromDatabase(string connectionString, string sqlStatement)
    {
        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(sqlStatement, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> data = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                data.Add(reader.GetName(i), reader.GetValue(i));
                            }

                            dataList.Add(data);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fehler beim Abrufen von Daten aus der Datenbank: " + ex.Message);
        }

        return dataList;
    }
    

    static void UpdateFormField(PdfDocument pdfDocument, string fieldName, string value, float fontSize, PdfFont font)
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
}