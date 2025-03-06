using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
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
    
    [ObservableProperty]
    private Persona? _trabajadorSeleccionado;

    [RelayCommand]
    private void HacerZoom(double factor)
    {
        Zoom = factor;
        Console.WriteLine("Zoom Realizado");
    }
    
    [RelayCommand]
    private void Salir()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
    
    [RelayCommand]
    private void Nuevo()
    {
        Trabajadores.Clear();  // 🔥 Vacía la lista de trabajadores
        GuardarPersona();  // 💾 Guarda el JSON vacío para persistencia
        Console.WriteLine("Lista de trabajadores limpiada.");
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
    private void EliminarTrabajador()
    {
        if (TrabajadorSeleccionado != null && Trabajadores.Contains(TrabajadorSeleccionado))
        {
            Trabajadores.Remove(TrabajadorSeleccionado);
            Console.WriteLine($"Trabajador {TrabajadorSeleccionado.Nombre} eliminado.");

            // Guardamos automáticamente la lista tras eliminar
            GuardarPersona();
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