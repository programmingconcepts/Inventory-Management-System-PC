using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Inventory_Management_System_PC.Commands;
using Inventory_Management_System_PC.Models;

namespace Inventory_Management_System_PC.ViewModels
{
    public class AddInvoiceViewModel : BaseViewModel
    {
        InventoryDBContext db = new InventoryDBContext();

        private ObservableCollection<Customer> _customers;
        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set
            {
                _customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;

                if (value != null)
                {
                    CustomerName = SelectedCustomer.CustomerName;
                    PhoneNumber = SelectedCustomer.PhoneNumber;
                    Address = SelectedCustomer.Address;
                }
                else
                {
                    CustomerName = null;
                    PhoneNumber = null;
                    Address = null;
                }

                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }

        private string _customerName;
        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                _customerName = value;
                OnPropertyChanged(nameof(CustomerName));
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        private ObservableCollection<Item> _items;
        public ObservableCollection<Item> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private Item _selectedItem;
        public Item SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if(value != null)
                {
                    SalePrice = value.Price;
                }
                else
                {
                    SalePrice = 0;
                }
                    OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private double _quantity;
        public double Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        private string _measuringUnit;
        public string MeasuringUnit
        {
            get { return _measuringUnit; }
            set
            {
                _measuringUnit = value;
                OnPropertyChanged(nameof(MeasuringUnit));
            }
        }

        private double _salePrice;
        public double SalePrice
        {
            get { return _salePrice; }
            set
            {
                _salePrice = value;
                OnPropertyChanged(nameof(SalePrice));
            }
        }

        private ObservableCollection<InvoiceItem> _invoiceItems;
        public ObservableCollection<InvoiceItem> InvoiceItems
        {
            get { return _invoiceItems; }
            set
            {
                if(InvoiceItems != null)
                {
                    InvoiceItems.CollectionChanged -= InvoiceItems_CollectionChanged;
                }

                _invoiceItems = value;

                if(value != null)
                {
                    InvoiceItems.CollectionChanged += InvoiceItems_CollectionChanged;
                }

                UpdateTotal();
                OnPropertyChanged(nameof(InvoiceItems));
            }
        }

        private void InvoiceItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateTotal();
        }

        private double _extraCharges;
        public double ExtraCharges
        {
            get { return _extraCharges; }
            set
            {
                _extraCharges = value;
                UpdateTotal();
                OnPropertyChanged(nameof(ExtraCharges));
            }
        }

        private double _discount;
        public double Discount
        {
            get { return _discount; }
            set
            {
                _discount = value;
                UpdateTotal();
                OnPropertyChanged(nameof(Discount));
            }
        }

        private double _paidAmount;
        public double PaidAmount
        {
            get { return _paidAmount; }
            set
            {
                _paidAmount = value;
                UpdateTotal();
                OnPropertyChanged(nameof(PaidAmount));
            }
        }

        private double _invoiceTotal;
        public double InvoiceTotal
        {
            get { return _invoiceTotal; }
            set
            {
                _invoiceTotal = value;
                OnPropertyChanged(nameof(InvoiceTotal));
            }
        }

        private void UpdateTotal()
        {
            double total = 0;
            if (InvoiceItems != null)
            {
                total += InvoiceItems.Sum(i => i.Quantity * i.SalePrice) - Discount + ExtraCharges;
                InvoiceTotal = total;
            }
        }


        public ICommand AddInvoiceItemCommand { get; }
        public ICommand SaveCommand { get; }
        public AddInvoiceViewModel()
        {
            AddInvoiceItemCommand = new RelayCommand(AddInvoiceItem, CanAddInvoiceItem);
            SaveCommand = new RelayCommand(Save, CanSave);

            LoadCustomers();
            LoadItems();
        }

        private void LoadCustomers()
        {
            Customers = new ObservableCollection<Customer>(db.Customers.OrderBy(c => c.CustomerName));
        }

        private void LoadItems()
        {
            var items = db.Items.ToList().
                Select(i =>
                {
                    i.StockValue = db.Stocks.Where(s => s.ItemId == i.ItemId).Select(s => s.StockValue).
                    DefaultIfEmpty(0).
                    Sum();
                    return i;
                }).Where(i => i.StockValue > 0).ToList();

            Items = new ObservableCollection<Item>(items);
        }

        private bool CanAddInvoiceItem (object parameter)
        {
            return SelectedItem != null && Quantity > 0;
        }

        private void AddInvoiceItem (object parameter)
        {
            InvoiceItem invoiceItem = new InvoiceItem
            {
                Item = SelectedItem,
                ItemId = SelectedItem.ItemId,
                Quantity = Quantity,
                SalePrice = SalePrice
            };

            if (InvoiceItems == null)
            {
                InvoiceItems = new ObservableCollection<InvoiceItem>();
            }
            InvoiceItems.Add(invoiceItem);
        }

        private bool CanSave (object parameter)
        {
            return (SelectedCustomer != null || (CustomerName != null && PhoneNumber != null)) && InvoiceItems != null;
        }

        private void Save (object parameter)
        {
            if(SelectedCustomer == null)
            {
                var customer = new Customer
                {
                    CustomerName = CustomerName,
                    PhoneNumber = PhoneNumber,
                    Address = Address
                };

                db.Customers.Add(customer);
                db.SaveChanges();
                SelectedCustomer = customer;
            }


            var transaction = db.Database.BeginTransaction();

            try
            {

                var invoice = new Invoice
                {
                    CustomerId = SelectedCustomer.CustomerId,
                    Date = DateTime.Now,
                    ExtraCharges = Convert.ToDecimal(ExtraCharges),
                    Discount = Convert.ToDecimal(Discount),
                    PaidAmount = Convert.ToDecimal(PaidAmount)
                };

                db.Invoices.Add(invoice);
                db.SaveChanges();

                foreach (var item in InvoiceItems)
                {
                    var invoiceItem = new InvoiceItem
                    {
                        InvoiceId = invoice.InvoiceId,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        SalePrice = item.SalePrice
                    };
                    db.InvoiceItems.Add(invoiceItem);

                    

                    double qtyToDeduct = item.Quantity;

                    while (qtyToDeduct > 0)
                    {
                        var stock = db.Stocks.FirstOrDefault(s => s.ItemId == item.ItemId && s.StockValue > 0);

                        if (stock != null)
                        {
                            if (stock.StockValue >= item.Quantity)
                            {
                                stock.StockValue -= item.Quantity;
                                qtyToDeduct = 0;
                            }
                            else
                            {
                                qtyToDeduct -= stock.StockValue;
                                stock.StockValue = 0;
                            }
                        }
                        db.Entry(stock).State = System.Data.Entity.EntityState.Modified;
                    }

                    
                }
                db.SaveChanges();

                transaction.Commit();

                MessageBox.Show("Invoice Saved Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                transaction.Rollback();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

    }
}
