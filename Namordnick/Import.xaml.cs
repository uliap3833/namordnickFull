using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Word = Microsoft.Office.Interop.Word;

namespace Namordnick
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Import : Window
    {

        List<Material> AllMAterial = Const.BD.Material.ToList();
        List<Supplier> AllSupller = Const.BD.Supplier.ToList();
        List<MaterialType> AllMaterialType = Const.BD.MaterialType.ToList();
        public Import()
        {
            InitializeComponent();
        }

        private void BFirst_Click(object sender, RoutedEventArgs e)
        {
            Word.Application application1 = new Word.Application();
            Word.Document document1 = application1.Documents.Add();

            foreach (var item in AllMAterial)
            {
                Word.Paragraph TitleParagraph = document1.Paragraphs.Add();
                Word.Range TitleRange = TitleParagraph.Range;
                TitleRange.Text = item.TitleBox;
                TitleParagraph.set_Style("Заголовок");
                TitleRange.InsertParagraphAfter();

                Word.Paragraph CostParagraph = document1.Paragraphs.Add();
                Word.Range CostRange = CostParagraph.Range;
                CostRange.Text = "Цена услуги: " + Convert.ToString(item.Cost) + " руб.";
                CostRange.Font.Color = Word.WdColor.wdColorBlue;
                TitleRange.InsertParagraphAfter();

                Word.Paragraph CountParagraph = document1.Paragraphs.Add();
                Word.Range CountRange = CostParagraph.Range;
                CountRange.Text = "Мнимальное количество: " + item.MinCount + " " + item.Unit;
                CountRange.InsertParagraphAfter();

                Word.Paragraph SuplertParagraph = document1.Paragraphs.Add();
                Word.Range SuplerRange = SuplertParagraph.Range;
                SuplerRange.Text = "Поставщики: " + item.SuplerStr;
                SuplerRange.InsertParagraphAfter();

                Word.Paragraph StockParagraph = document1.Paragraphs.Add();
                Word.Range StockrRange = StockParagraph.Range;
                StockrRange.Text = item.StockStr;
                StockrRange.InsertParagraphAfter();


                Word.Paragraph ImgParagraph = document1.Paragraphs.Add();
                Word.Range ImgRange = ImgParagraph.Range;
                Word.InlineShape imageShape = ImgRange.InlineShapes.AddPicture(AppDomain.CurrentDomain.BaseDirectory + "..\\" + item.PhotoPath);
                imageShape.Width = imageShape.Height = 100;




                if (item != AllMAterial.LastOrDefault())
                {
                    document1.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
                }


            }
            application1.Visible = true;
        }

        private void BSecond_Click(object sender, RoutedEventArgs e)
        {
            Word.Application application2 = new Word.Application();
            Word.Document document2 = application2.Documents.Add();

            foreach (var item in AllMaterialSupller)
            {
                Word.Paragraph TitleMaterialParagraph = document2.Paragraphs.Add();
                Word.Range TitleMaterialRange = TitleMaterialParagraph.Range;
                TitleMaterialRange.Text = AllMAterial[item.MaterialID].Title;
                TitleMaterialRange.set_Style("Заголовок");
                TitleMaterialRange.InsertParagraphAfter();

                Word.Paragraph SuplearParagraph = document2.Paragraphs.Add();
                Word.Range SuplearRange = SuplearParagraph.Range;
                SuplearRange.Text = "Поставщики данной продукции:";
                SuplearRange.InsertParagraphAfter();

                Word.Paragraph TableParagraph = document2.Paragraphs.Add();
                Word.Range TablelRange = TableParagraph.Range;
                Word.Table SupplierTable;
                if (TemporaryMaterialSupplier.Count() != 0)
                {
                    SupplierTable = document2.Tables.Add(TablelRange, TemporaryMaterialSupplier.Count() + 1, 5);
                }
                else
                {
                    SupplierTable = document2.Tables.Add(TablelRange, 2, 5);
                }
                SupplierTable.Borders.InsideLineStyle = SupplierTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;

                Word.Range CellRangeTitle;
                CellRangeTitle = SupplierTable.Cell(1, 1).Range;
                CellRangeTitle.Text = "Название";
                CellRangeTitle = SupplierTable.Cell(1, 2).Range;
                CellRangeTitle.Text = "ИНН";
                CellRangeTitle = SupplierTable.Cell(1, 3).Range;
                CellRangeTitle.Text = "Дата начала";
                CellRangeTitle = SupplierTable.Cell(1, 4).Range;
                CellRangeTitle.Text = "Рейтинг качества";
                CellRangeTitle = SupplierTable.Cell(1, 5).Range;
                CellRangeTitle.Text = "Тип поставщика";
                SupplierTable.Rows[1].Range.Font.Color = Word.WdColor.wdColorDarkRed;
                SupplierTable.Rows[1].Range.Font.Bold = 1;
                SupplierTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                Word.Range CellRange;

                for (int i = 0; i < TemporaryMaterialSupplier.Count(); i++)
                {
                    var CurrentMaterialSupplier = TemporaryMaterialSupplier[i];
                    CellRange = SupplierTable.Cell(i + 2, 1).Range;
                    CellRange.Text = AllSupller[TemporaryMaterialSupplier[i].SupplierID].Title;
                    CellRange = SupplierTable.Cell(i + 2, 2).Range;
                    CellRange.Text = AllSupller[TemporaryMaterialSupplier[i].SupplierID].INN;
                    CellRange = SupplierTable.Cell(i + 2, 3).Range;
                    CellRange.Text = AllSupller[TemporaryMaterialSupplier[i].SupplierID].StartDate.ToString();
                    CellRange = SupplierTable.Cell(i + 2, 4).Range;
                    CellRange.Text = AllSupller[TemporaryMaterialSupplier[i].SupplierID].QualityRating.ToString();
                    CellRange = SupplierTable.Cell(i + 2, 5).Range;
                    CellRange.Text = AllSupller[TemporaryMaterialSupplier[i].SupplierID].SupplierType;

                    SupplierTable.Rows[i + 1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                }

                if (item != AllMaterialSupller.LastOrDefault())
                {
                    document2.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
                }
            }
            application2.Visible = true;
        }

        private void BThrith_Click(object sender, RoutedEventArgs e)
        {
            Word.Application application3 = new Word.Application();
            Word.Document document3 = application3.Documents.Add();

            foreach (var item in AllMaterialType)
            {
                Word.Paragraph TitleMaterialParagraph = document3.Paragraphs.Add();
                Word.Range TitleMaterialRange = TitleMaterialParagraph.Range;
                TitleMaterialRange.Text = item.Title;
                TitleMaterialRange.set_Style("Заголовок");
                TitleMaterialRange.InsertParagraphAfter();

                Word.Paragraph SuplearParagraph = document3.Paragraphs.Add();
                Word.Range SuplearRange = SuplearParagraph.Range;
                SuplearRange.Text = "Материалы по данному типу:";
                SuplearRange.InsertParagraphAfter();

                Word.Paragraph TableParagraph = document3.Paragraphs.Add();
                Word.Range TablelRange = TableParagraph.Range;
                List<Material> TemporaryMaterial = AllMAterial.Where(x => x.MaterialTypeID == item.ID).ToList();
                Word.Table MaterialTable;
                if (TemporaryMaterial.Count() != 0)
                {
                    MaterialTable = document3.Tables.Add(TablelRange, TemporaryMaterial.Count() + 1, 4);
                }
                else
                {
                    MaterialTable = document3.Tables.Add(TablelRange, 2, 4);
                }
                MaterialTable.Borders.InsideLineStyle = MaterialTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;

                Word.Range CellRangeTitle;
                CellRangeTitle = MaterialTable.Cell(1, 1).Range;
                CellRangeTitle.Text = "Название";
                CellRangeTitle = MaterialTable.Cell(1, 2).Range;
                CellRangeTitle.Text = "Количество на складе";
                CellRangeTitle = MaterialTable.Cell(1, 3).Range;
                CellRangeTitle.Text = "Цена";
                CellRangeTitle = MaterialTable.Cell(1, 4).Range;
                CellRangeTitle.Text = "Картинка";

                MaterialTable.Rows[1].Range.Font.Color = Word.WdColor.wdColorDarkRed;
                MaterialTable.Rows[1].Range.Font.Bold = 1;
                MaterialTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                Word.Range CellRange;

                for (int i = 0; i < TemporaryMaterial.Count(); i++)
                {
                    var CurrentMaterial = TemporaryMaterial[i];
                    CellRange = MaterialTable.Cell(i + 2, 1).Range;
                    CellRange.Text = CurrentMaterial.Title;
                    CellRange = MaterialTable.Cell(i + 2, 2).Range;
                    CellRange.Text = CurrentMaterial.CountInStock + "";
                    CellRange = MaterialTable.Cell(i + 2, 3).Range;
                    CellRange.Text = CurrentMaterial.Cost + "";
                    CellRange = MaterialTable.Cell(i + 2, 4).Range;
                    Word.InlineShape imageShape = CellRange.InlineShapes.AddPicture(AppDomain.CurrentDomain.BaseDirectory + "..\\" + CurrentMaterial.PhotoPath);
                    imageShape.Width = imageShape.Height = 50;


                    MaterialTable.Rows[i + 1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                }

                if (item != AllMaterialType.LastOrDefault())
                {
                    document3.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
                }


            }
            application3.Visible = true;
        }

        private void BFour_Click(object sender, RoutedEventArgs e)
        {
            Word.Application application4 = new Word.Application();
            Word.Document document4 = application4.Documents.Add();

            foreach (var item in AllMaterialType)
            {
                Word.Paragraph TitleParagraph = document4.Paragraphs.Add();
                Word.Range TitleRange = TitleParagraph.Range;
                TitleRange.Text = item.ID + " " + item.Title;
                TitleParagraph.set_Style("Заголовок");
                TitleRange.InsertParagraphAfter();

                Word.Paragraph DeffectedParagraph = document4.Paragraphs.Add();
                Word.Range DeffectedRange = DeffectedParagraph.Range;
                if (item.DefectedPercent == null)
                {
                    DeffectedRange.Text = "Процент деффекта: нет";
                }
                else
                    DeffectedRange.Text = "Процент деффекта: " + item.DefectedPercent;
                DeffectedRange.InsertParagraphAfter();






                if (item != AllMaterialType.LastOrDefault())
                {
                    document4.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
                }


            }
            application4.Visible = true;
        }

        private void BFift_Click(object sender, RoutedEventArgs e)
        {
            Word.Application application5 = new Word.Application();
            Word.Document document5 = application5.Documents.Add();

            Word.Paragraph TableParagraph = document5.Paragraphs.Add();
            Word.Range TablelRange = TableParagraph.Range;
            Word.Table MaterialTable;
            MaterialTable = document5.Tables.Add(TablelRange, AllSupller.Count() + 1, 4);

            MaterialTable.Borders.InsideLineStyle = MaterialTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;

            Word.Range CellRangeTitle;
            CellRangeTitle = MaterialTable.Cell(1, 1).Range;
            CellRangeTitle.Text = "Название поставщика";
            CellRangeTitle = MaterialTable.Cell(1, 2).Range;
            CellRangeTitle.Text = "ИНН";
            CellRangeTitle = MaterialTable.Cell(1, 3).Range;
            CellRangeTitle.Text = "Тип";
            CellRangeTitle = MaterialTable.Cell(1, 4).Range;
            CellRangeTitle.Text = "Рейтинг";

            Word.Range CellRange;

            for (int i = 0; i < AllSupller.Count(); i++)
            {
                var CurrentMaterial = AllSupller[i];
                CellRange = MaterialTable.Cell(i + 2, 1).Range;
                CellRange.Text = AllSupller[i].Title;
                CellRange = MaterialTable.Cell(i + 2, 2).Range;
                CellRange.Text = AllSupller[i].INN;
                CellRange = MaterialTable.Cell(i + 2, 3).Range;
                CellRange.Text = AllSupller[i].SupplierType;
                CellRange = MaterialTable.Cell(i + 2, 4).Range;
                CellRange.Text = AllSupller[i].QualityRating + "";

                MaterialTable.Rows[i + 1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            }

            application5.Visible = true;
        }
    }
}
