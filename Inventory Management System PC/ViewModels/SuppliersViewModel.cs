using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Inventory_Management_System_PC.Models;
using Inventory_Management_System_PC.Commands;
using System.Diagnostics;

namespace Inventory_Management_System_PC.ViewModels
{
    public class SuppliersViewModel: BaseViewModel
    {
        private ObservableCollection<Supplier> suppliers;
        public ObservableCollection<Supplier> Suppliers
        {
            get { return suppliers; }
            set
            {
                suppliers = value;
                OnPropertyChanged(nameof(Suppliers));
            }
        }

        private Supplier selectedSupplier;
        public Supplier SelectedSupplier
        {
            get { return selectedSupplier; }
            set
            {
                selectedSupplier = value;
                if(value != null)
                {
                    Name = value.Name;
                    Phone = value.Phone;
                    Address = value.Address;
                }
                OnPropertyChanged(nameof(SelectedSupplier));
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string phone;
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        private string address;
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand UpdateCommand { get; }


        public SuppliersViewModel()
        {
            Name = "";
            Phone = "";
            Address = "";
            SaveCommand = new RelayCommand(SaveSupplier, CanSaveSupplier);
            UpdateCommand = new RelayCommand(UpdateSupplier, CanUpdateSupplier);
            LoadSuppliers();
        }

        private bool CanSaveSupplier(object obj)
        {
            return !string.IsNullOrEmpty(Name);
        }

        private void SaveSupplier(object obj)
        {
            Supplier supplier = new Supplier
            {
                Name = Name,
                Phone = Phone,
                Address = Address
            };
            
            InventoryDBContext db = new InventoryDBContext();
            db.Suppliers.Add(supplier);
            db.SaveChanges();

            ClearFields();
            LoadSuppliers();
        }

        private bool CanUpdateSupplier(object obj)
        {
            return SelectedSupplier != null && !string.IsNullOrEmpty(Name);
        }

        private void UpdateSupplier(object obj)
        {
            
            InventoryDBContext db = new InventoryDBContext();
            var supplier = db.Suppliers.Find(SelectedSupplier.SupplierId);

            supplier.Name = Name;
            supplier.Phone = Phone;
            supplier.Address = Address;

            db.Entry(supplier).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            ClearFields();
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            InventoryDBContext db = new InventoryDBContext();
            Suppliers = new ObservableCollection<Supplier>(db.Suppliers);
        }

        private void ClearFields()
        {
            Name = "";
            Phone = "";
            Address = "";
        }
    }
}
