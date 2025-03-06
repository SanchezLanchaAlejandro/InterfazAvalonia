using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using AvaloniaPrueba.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaPrueba.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Persona> _trabajadores;

    [ObservableProperty]
    private double _zoom = 1.0;
    
    // Propiedad para enlazar los datos del formulario
    [ObservableProperty]
    private Persona _nuevoTrabajador = new Persona();

    [RelayCommand]
    private void HacerZoom(double factor)
    {
        Zoom = factor;
        Console.WriteLine("Zoom Realizado");
    }

    [RelayCommand]
    private void CrearPersona()
    {
        Console.WriteLine("He creado una Persona");
    }

    [RelayCommand]
    private void GuardarPersona()
    {
        string json = JsonSerializer.Serialize(Trabajadores);
        using (StreamWriter sw = File.CreateText(AppDomain.CurrentDomain.BaseDirectory + "trabajadores.json"))
        {
            sw.WriteLine(json);
            Console.WriteLine("Archivo Guardado");
        }
    }

    [RelayCommand]
    private void Mostrar()
    {
        Console.WriteLine("Mostrada una persona");
    }
    
    [RelayCommand]
    private void Editar()
    {
        Console.WriteLine("Editada una persona");
    }

    // Comando para guardar el nuevo trabajador
    [RelayCommand]
    private void GuardarNuevoTrabajador()
    {
        if (NuevoTrabajador != null)
        {
            // Agregar una nueva instancia basada en los datos del formulario
            Trabajadores.Add(new Persona
            {
                Nombre = NuevoTrabajador.Nombre,
                Edad = NuevoTrabajador.Edad,
                Ficticio = NuevoTrabajador.Ficticio
            });
            Console.WriteLine("Nuevo trabajador agregado");

            // Reinicializar el objeto para limpiar el formulario
            NuevoTrabajador = new Persona();
        }
    }

    public MainWindowViewModel()
    {
        Trabajadores = CargarTrabajadores();
    }

    public ObservableCollection<Persona> CargarTrabajadores()
    {
        ObservableCollection<Persona> trabajadores;
        using (StreamReader sr = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + "trabajadores.json"))
        {
            trabajadores = JsonSerializer.Deserialize<ObservableCollection<Persona>>(sr.ReadToEnd());
        }
        return trabajadores;
    }
}