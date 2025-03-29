﻿using Inventory_Management_System_PC.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Inventory_Management_System_PC.Commands;
using System.Windows;

namespace Inventory_Management_System_PC.ViewModels
{
    public class AddSupplyViewModel: BaseViewModel
    {
        private InventoryDBContext db = new InventoryDBContext();

        private ObservableCollection<Supplier> _suppliers;
        public ObservableCollection<Supplier> Suppliers
        {
            get { return _suppliers; }
            set
            {
                _suppliers = value;
                OnPropertyChanged(nameof(Suppliers));
            }
        }

        private Supplier _selectedSupplier;
        public Supplier SelectedSupplier
        {
            get { return _selectedSupplier; }
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged(nameof(SelectedSupplier));
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
                    MeasuringUnitText = "(" + value.MeasuringUnit + "):";
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

        private string _measuringUnitText;
        public string MeasuringUnitText
        {
            get { return _measuringUnitText; }
            set
            {
                _measuringUnitText = value;
                OnPropertyChanged(nameof(MeasuringUnitText));
            }
        }

        private double _purchasePrice;
        public double PurchasePrice
        {
            get { return _purchasePrice; }
            set
            {
                _purchasePrice = value;
                OnPropertyChanged(nameof(PurchasePrice));
            }
        }

        private ObservableCollection<SupplyDetail> _supplyDetails;
        public ObservableCollection<SupplyDetail> SupplyDetails
        {
            get { return _supplyDetails; }
            set
            {
                _supplyDetails = value;
                OnPropertyChanged(nameof(SupplyDetails));
            }
        }

        private double _totalAmount;
        public double TotalAmount
        {
            get { return _totalAmount; }
            set
            {
                _totalAmount = value;
                OnPropertyChanged(nameof(TotalAmount));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand AddSupplyDetailCommand { get; }

        public AddSupplyViewModel()
        {
            SaveCommand = new RelayCommand(SaveSupply, CanSaveSupply);
            AddSupplyDetailCommand = new RelayCommand(AddSupplyDetail, CanAddSupplyDetail);
            TotalAmount = 0;

            SupplyDetails = new ObservableCollection<SupplyDetail>();

            LoadItems();
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            Suppliers = new ObservableCollection<Supplier>(db.Suppliers);
        }

        private void LoadItems()
        {
            Items = new ObservableCollection<Item>(db.Items);
        }

        private bool CanSaveSupply(object parameter)
        {
            return SelectedSupplier != null && SupplyDetails.Count > 0;
        }

        private void SaveSupply(object parameter)
        {
            var transaction = db.Database.BeginTransaction();

            try
            {
                Supply supply = new Supply
                {
                    SupplierId = SelectedSupplier.SupplierId,
                    Date = DateTime.Now,
                    SupplyDetails = SupplyDetails
                };
                db.Supplies.Add(supply);
                db.SaveChanges();

                foreach(var supplyDetail in SupplyDetails)
                {

                    var newStock = new Stock
                    {
                        ItemId = supplyDetail.ItemId,
                        SupplyDetailId = supplyDetail.SupplyDetailId,
                        StockValue = supplyDetail.Quantity
                    };

                    db.Stocks.Add(newStock);
                }

                db.SaveChanges();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show("An error occurred while saving the supply" + Environment.NewLine + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            

            MessageBox.Show("Supply saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool CanAddSupplyDetail(object parameter)
        {
            return SelectedItem != null && Quantity > 0 && PurchasePrice > 0;
        }

        private void AddSupplyDetail(object parameter)
        {
            var supplyDetail = new SupplyDetail
            {
                ItemId = SelectedItem.ItemId,
                Quantity = Quantity,
                PurchasePrice = PurchasePrice,
                Item = SelectedItem
                
            };
            SupplyDetails.Add(supplyDetail);

            RefreshTotal();
        }

        private void RefreshTotal ()
        {
            TotalAmount = SupplyDetails.Sum(sd => sd.Quantity * sd.PurchasePrice);
        }
    }
}
