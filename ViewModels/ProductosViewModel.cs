using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using FerreteraBD.Models;
using FerreteraBD.Views;
using GalaSoft.MvvmLight.Command;

namespace FerreteraBD.ViewModels
{
    internal class ProductosViewModel : INotifyPropertyChanged
    {
        FerreteriaContext context = new FerreteriaContext();

        // PROPIEDADES

        public ICommand AgregarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }

        //Propiedad para mostrar los errores
        private string error;

        public string Error
        {
            get { return error; }
            set { error = value;
                Actualizar("Error");
            }
        }

        //Producto que nos servira para agregarlo, editarlo o eliminarlo
        private Producto producto;

        public Producto Producto
        {
            get { return producto; }
            set { producto = value;
                Actualizar("Producto");
            }
        }

        //Lista para almacenar todos los productos y manipularlos
        private ObservableCollection<Producto> lista;

        public ObservableCollection<Producto> ListaProductos
        {
            get { return lista; }
            set { lista = value;
                Actualizar("ListaProductos");
            }
        }

        //Lista de secciones 
        private List<Seccion> listaseccion;

        public List<Seccion> ListaSeccion
        {
            get { return listaseccion; }
            set { listaseccion = value; 
                Actualizar("ListaSeccion");
            }
        }

        public string Modo { get; set; } = "Ver";
        public UserControl Vista { get; set; }
        DetallesView detallesview;


        // CONSTRUCTOR
        public ProductosViewModel()
        {
            GuardarCommand = new RelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
            EditarCommand = new RelayCommand(Editar);
            AgregarCommand = new RelayCommand(Agregar);
            EliminarCommand = new RelayCommand(Eliminar);
            RellenarListaProductos();
            ListaSeccion = new(context.Seccions.OrderBy(x => x.Nombre));

        }


        // METODOS

        private void Iniciar()
        {
            detallesview = new DetallesView() { DataContext = this };

            Vista = detallesview;
            Actualizar();
        }

        //CRUD

        public void Agregar()
        {
            Error = "";
            Producto = new Producto();
            Modo = "Agregar";
            Vista = detallesview;
            Actualizar();
        }
        public void Editar()
        {
            Error = "";
            Modo = "Editar";
            Vista = detallesview;
            Actualizar();
        }
        public void Guardar()
        {
            if (Producto != null)
            {
                Error = "";
                if (string.IsNullOrWhiteSpace(Producto.Nombre))
                {
                    Error += "Escriba el nombre del empleado" + Environment.NewLine;
                }
                if (Producto.Precio <= 0)
                {
                    Error += "El sueldo del empleado debe ser mayor a $0.00" + Environment.NewLine;
                }
                Actualizar("Error");

                if (Error == "")
                {
                    if (producto.Id == 0)
                    {
                        context.Productos.Add(Producto);
                    }
                    else
                    {
                        context.Update(Producto);
                    }
                    context.SaveChanges();
                    context.Entry(Producto).Reload();
                    Actualizar("");
                    RellenarListaProductos();
                    Cancelar();
                }
            }

            }
            public void Cancelar()
        {
            try
            {
                if (Producto != null)
                {
                    context.Entry(Producto).Reload();
                }
                Error = "";
                Vista = detallesview;
                Actualizar("Vista");
            }
            catch (Exception ex)
            {
                Error = ex.ToString();
            }

        }

        private void Eliminar()
        {
            try
            {
                Error = "";
                context.Productos.Remove(context.Productos.FirstOrDefault(x => x.Id == producto.Id));
                context.SaveChanges();
                Actualizar("ListaProductos");
                RellenarListaProductos();
            }
            catch (Exception)
            {
                Error = "Selecciona un elemento para poder eliminarlo";
            }
            
        }

        //Metodo para rellenar la lista de productos, tambien nos servira para actualizarla
        public void RellenarListaProductos()
        {
            ListaProductos = new(context.Productos.OrderBy(x => x.Nombre));
        }


        //Metodo para actualizar las propiedades
        public void Actualizar(string? prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        //Interfaz para los eventos
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
