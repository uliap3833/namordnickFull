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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Word = Microsoft.Office.Interop.Word;

namespace Namordnick
{
    /// <summary>
    /// Логика взаимодействия для ServiceList.xaml
    /// </summary>
    public partial class ServiceList : Page
    {
        List<Product> DB = Const.BD.Product.ToList();
        PageChange pc = new PageChange(Const.BD.Product.ToList().Count);
        public ServiceList()
        {
            InitializeComponent();
            LVCelebration.ItemsSource = Const.BD.Product.ToList();
        }


        public void Filt()
        {
            List<Product> newDB = new List<Product>();
            LVCelebration.Items.Clear();
            DB =Const.BD.Product.ToList();

            pc.CountInList = DB.Count;
            pc.CurrentPage = 0;
            for (int i = (pc.CurrentPage - 1) * 20; i < pc.CurrentPage * 20; i++)
            {
                if (DB.Count > i && i >= 0)
                {
                    LVCelebration.Items.Add(DB[i]);
                }
            }
        }
        private void BtnChangePrice_Click(object sender, RoutedEventArgs e)
        {

            List<Product> ppp = new List<Product>();
            foreach (Product pp in LVCelebration.SelectedItems)
            {
                ppp.Add(pp);
            }
            ChangePriceForAgent window = new ChangePriceForAgent(ppp as List<Product>);
            window.Visibility = Visibility.Visible;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
            LVCelebration.Items.Refresh();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddoOrRedactProduct window = new AddoOrRedactProduct();
            window.Visibility = Visibility.Visible;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
            Filt();
        }

        private void BtnChange_Click(object sender, RoutedEventArgs e)
        {
            Product p = (Product)LVCelebration.SelectedItem;
            AddoOrRedactProduct window = new AddoOrRedactProduct(p);
            window.Visibility = Visibility.Visible;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
            Filt();
        }

        private void GoPage_Click(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            switch (tb.Uid)
            {
                case "Prev":
                    pc.CurrentPage--;
                    break;
                case "Next":
                    pc.CurrentPage++;
                    break;
                default:
                    pc.CurrentPage = Convert.ToInt32(tb.Text);
                    break;
            }
            LVCelebration.Items.Clear();
            for (int i = (pc.CurrentPage - 1) * 20; i < pc.CurrentPage * 20; i++)
            {
                if (DB.Count > i)
                {
                    LVCelebration.Items.Add(DB[i]);
                }
            }
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            Word.Application app = new Word.Application();
            Word.Document doc = app.Documents.Add();

            Word.Paragraph pp = doc.Paragraphs.Add();
            Word.Range pr = pp.Range;
            pr.Text = "Категория: " + CBCol.Text + (TBOXSearch.Text.Length > 0 ? ". Ключевое слово: " + TBOXSearch.Text : ".");
            pp.set_Style("Заголовок");
            pr.InsertParagraphAfter();

            Word.Paragraph tableParagraph = doc.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table table = doc.Tables.Add(tableRange, DB.Count + 1, 6);
            table.Borders.InsideLineStyle = table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            table.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            Word.Range cellRange;
            table.Cell(1, 1).Range.Text = "Иконка";
            table.Cell(1, 2).Range.Text = "Название";
            table.Cell(1, 3).Range.Text = "Категория";
            table.Cell(1, 4).Range.Text = "Артикль";
            table.Cell(1, 5).Range.Text = "Материалы";
            table.Cell(1, 6).Range.Text = "Цена";
            table.Rows[1].Range.Bold = 1;
            table.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;


            for (int i = 0; i < DB.Count; i++)
            {
                Product p = DB[i];
                Word.InlineShape image = table.Cell(i + 2, 1).Range.InlineShapes.AddPicture(AppDomain.CurrentDomain.BaseDirectory + p.GetIcon);
                image.Width = image.Height = 50;
                table.Cell(i + 2, 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                table.Cell(i + 2, 2).Range.Text = p.Title;
                table.Cell(i + 2, 3).Range.Text = p.ProductType.Title;
                table.Cell(i + 2, 4).Range.Text = p.ArticleNumber;
                table.Cell(i + 2, 5).Range.Text = p.GetMaterials;
                table.Cell(i + 2, 6).Range.Text = p.CostProduct;
            }
            app.Visible = true;
            doc.SaveAs2(@"..\Products_" + CBCol.Text + (TBOXSearch.Text.Length > 0 ? "_Ключевое_слово_" + TBOXSearch.Text : "") + ".docx");

        }

        private void BtnReportMaterials_Click(object sender, RoutedEventArgs e)
        {
            Word.Application app = new Word.Application();
            Word.Document doc = app.Documents.Add();

            Word.Paragraph pp = doc.Paragraphs.Add();
            Word.Range pr = pp.Range;
            pr.Text = "Категория: " + CBCol.Text + (TBOXSearch.Text.Length > 0 ? ". Ключевое слово: " + TBOXSearch.Text : ".");
            pp.set_Style("Заголовок");
            pr.InsertParagraphAfter();

            foreach (Product p in DB)
            {
                Word.Paragraph productParagraph = doc.Paragraphs.Add();
                Word.Range productRange = productParagraph.Range;
                productRange.Text = p.Title;
                productParagraph.set_Style("Заголовок");
                productRange.InsertParagraphAfter();

                Word.Paragraph tableParagraph = doc.Paragraphs.Add();
                Word.Range tableRange = tableParagraph.Range;

                List<ProductMaterial> pm = Const.BD.ProductMaterial.Where(x => x.ProductID == p.ID).ToList();
                if (pm.Count > 0)
                {
                    Word.Table table = doc.Tables.Add(tableRange, pm.Count + 1, 2);
                    table.Borders.InsideLineStyle = table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                    table.Cell(1, 1).Range.Text = "Название";
                    table.Cell(1, 2).Range.Text = "Количество";
                    table.Rows[1].Range.Bold = 1;
                    table.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    for (int i = 0; i < pm.Count; i++)
                    {
                        ProductMaterial prodMat = pm[i];
                        table.Cell(i + 2, 1).Range.Text = prodMat.Material.Title;
                        table.Cell(i + 2, 2).Range.Text = "" + prodMat.Count;

                    }
                }
                else
                {
                    tableRange.Text = "Нет данных о материалах.";
                }
                if (p != DB.LastOrDefault())
                {
                    doc.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
                }
            }
            app.Visible = true;
            doc.SaveAs2(@"..\ProductMaterials_" + CBCol.Text + (TBOXSearch.Text.Length > 0 ? "_Ключевое_слово_" + TBOXSearch.Text : "") + ".docx");
        }

        private void BtnReportMaterialsInfo_Click(object sender, RoutedEventArgs e)
        {
            List<MaterialType> materialType = Const.BD.MaterialType.ToList();
            Word.Application app = new Word.Application();
            Word.Document doc = app.Documents.Add();

            foreach (MaterialType p in materialType)
            {
                Word.Paragraph productParagraph = doc.Paragraphs.Add();
                Word.Range productRange = productParagraph.Range;
                productRange.Text = p.Title;
                productParagraph.set_Style("Заголовок");
                productRange.InsertParagraphAfter();

                Word.Paragraph tableParagraph = doc.Paragraphs.Add();
                Word.Range tableRange = tableParagraph.Range;

                List<Material> pm = Const.BD.Material.Where(x => x.MaterialTypeID == p.ID).ToList();
                if (pm.Count > 0)
                {
                    Word.Table table = doc.Tables.Add(tableRange, pm.Count + 1, 5);
                    table.Borders.InsideLineStyle = table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                    table.Cell(1, 1).Range.Text = "Название";
                    table.Cell(1, 2).Range.Text = "Количество в упаковке";
                    table.Cell(1, 3).Range.Text = "Ед. изм.";
                    table.Cell(1, 4).Range.Text = "Количество на складе";
                    table.Cell(1, 5).Range.Text = "Цена";

                    table.Rows[1].Range.Bold = 1;
                    table.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    for (int i = 0; i < pm.Count; i++)
                    {
                        Material m = pm[i];
                        table.Cell(i + 2, 1).Range.Text = m.Title;
                        table.Cell(i + 2, 2).Range.Text = "" + m.CountInPack;
                        table.Cell(i + 2, 3).Range.Text = m.Unit;
                        table.Cell(i + 2, 4).Range.Text = "" + m.CountInStock;
                        table.Cell(i + 2, 5).Range.Text = "" + m.Cost;


                    }
                }
                else
                {
                    tableRange.Text = "Нет материалов их данного типа";
                }
                if (p != materialType.LastOrDefault())
                {
                    doc.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
                }
            }
            app.Visible = true;
            doc.SaveAs2(@"..\MaterialsInfo.docx");
        }
    }
}
