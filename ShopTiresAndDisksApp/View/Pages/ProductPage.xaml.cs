using ShopTiresAndDisksApp.Model;
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

namespace ShopTiresAndDisksApp.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {

        bool check = true;
        int page = 1;
        Core db = new Core();
        int materialId;
        //Количество элементов на странице
        int countElement = 10;
        public ProductPage()
        {
            InitializeComponent();

            var arrayMaterial = new List<MaterialType>
            {
             new MaterialType
            {
            ID = 0,
            Title = "Все"
            }
            };
            arrayMaterial.AddRange(db.context.MaterialType.ToList());
            FilterComboBox.ItemsSource = arrayMaterial;
            FilterComboBox.DisplayMemberPath = "Title";
            FilterComboBox.SelectedValuePath = "ID";




            UpdateUI();
        }


        private void UpdateUI()
        {
            if (GetRows().Count > 10)
            {

                List<Product> displayProduct = GetRows().Skip((page - 1) * countElement).Take(countElement).ToList();
                DisplayPagination(page);
                ProductListView.ItemsSource = displayProduct;

            }
            else
            {
                PaginationListView.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Формирование данных для вывода 
        /// </summary>
        /// <returns>
        /// Возвращает все данные из таблицы
        /// </returns>
        private List<Product> GetRows()
        {

            List<Product> arrayProduct = db.context.Product.ToList();

            //List<MaterialType> arrayMaterialTypes = db.context.MaterialType.ToList();
            if (SearchTextBox.Text != String.Empty && !String.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                arrayProduct = arrayProduct.Where(x => x.Title.ToUpper().Contains(SearchTextBox.Text.ToUpper()) || x.MaterialList.ToUpper().Contains(SearchTextBox.Text.ToUpper())).ToList();
            }




            if (SortComboBox.SelectedIndex == 1)
            {

                if (check)
                {
                    arrayProduct = arrayProduct.OrderByDescending(x => x.Title).ToList();
                }
                else
                {
                    arrayProduct = arrayProduct.OrderBy(x => x.Title).ToList();
                }


            }
            if (SortComboBox.SelectedIndex == 2)
            {
                if (check)
                {
                    arrayProduct = arrayProduct.OrderByDescending(x => x.CostProduct).ToList();
                }
                else
                {
                    arrayProduct = arrayProduct.OrderBy(x => x.CostProduct).ToList();
                }


            }

            if (materialId != 0)
            {

                arrayProduct = arrayProduct.Where(x => x.ProductMaterial.Join(Material).Material.MaterialTypeID == materialId).ToList();

            }
            else
            {
                arrayProduct = arrayProduct.ToList();
            }

            return arrayProduct;


        }
        /// <summary>
        /// Подсчёт количества страниц,в пагинции
        /// </summary>
        /// <returns>
        /// Возвращение целочисленного значения количества страниц
        /// </returns>
        private int GetPagesCount()
        {
            int countPage = 0;

            int count = GetRows().Count;
            if (count > countElement)
            {
                countPage = Convert.ToInt32(Math.Ceiling(count * 1.0 / countElement));
            }
            return countPage;
        }
        /// <summary>
        /// Вывод кнопок пагинации
        /// </summary>
        /// <param name="page">
        /// Количество страниц
        /// </param>
        public void DisplayPagination(int page)
        {
            List<PageItem> source = new List<PageItem>();
            for (int i = 1; i < GetPagesCount(); i++)
            {
                source.Add(new PageItem(i, i == page));
            }
            PaginationListView.ItemsSource = source;
            PrevTextBlock.Visibility = (page <= 1 ? Visibility.Hidden : Visibility.Visible);
            NextTextBlock.Visibility = (page >= GetPagesCount() ? Visibility.Hidden : Visibility.Visible);
        }

        private void PrevTextBlockMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (page <= 1)
            {
                page = 1;
                PrevTextBlock.Visibility = Visibility.Hidden;
            }
            else
            {
                page -= 1;
                PrevTextBlock.Visibility = Visibility.Visible;
            }
            UpdateUI();
        }

        private void NextTextBlockMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (page >= GetPagesCount())
            {
                page = GetPagesCount();
                NextTextBlock.Visibility = Visibility.Hidden;
            }
            else
            {
                page += 1;
                NextTextBlock.Visibility = Visibility.Visible;
            }
            UpdateUI();
        }


        private void TextBlockMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            page = Convert.ToInt32(textBlock.Text);
            UpdateUI();
        }

        private void SearchTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUI();
        }

        private void OrderButtonClick(object sender, RoutedEventArgs e)
        {   
            check = !check;
            if (check)
            {
                OrderButton.Content = "↓";
            }
            else
            {
                OrderButton.Content = "↑";
            }

            UpdateUI();
        }

        private void SortComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
                UpdateUI();
            

        }

        private void FilterComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            materialId = Convert.ToInt32(FilterComboBox.SelectedValue);
            UpdateUI();
        }
    }
}
