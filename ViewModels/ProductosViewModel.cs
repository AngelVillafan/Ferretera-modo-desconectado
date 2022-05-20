using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using FerreteraBD.Models;

namespace FerreteraBD.ViewModels
{
    internal class ProductosViewModel : INotifyPropertyChanged
    {
        FerreteriaContext context = new FerreteriaContext();

        // PROPIEDADES

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
                Actualizar("Lista");
            }
        }

        //Lista de secciones
        private List<Seccion> listaseccion;

        public List<Seccion> ListaSeccion
        {
            get { return listaseccion; }
            set { listaseccion = value; 
                Actualizar("ListaSexxion");
            }
        }

        private ContentControl vista;

        public ContentControl Vista
        {
            get { return vista; }
            set { vista = value; }
        }


        // CONSTRUCTOR
        public ProductosViewModel()
        {
            RellenarListaProductos();
            ListaSeccion = new(context.Seccions.OrderBy(x => x.Nombre));

        }


        // METODOS

        //CRUD

        public void Agregar()
        {

        }
        public void Editar()
        {

        }
        public void Guardar()
        {

        }
        public void Cancelar()
        {

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
