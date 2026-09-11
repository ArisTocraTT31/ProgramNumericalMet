using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramNumericalMet.Models
{
    public partial class Zadania
    {
        public int Id {  get; set; }
        public string Title { get; set; }
        public static IReadOnlyList<Zadania> All { get; } = new List<Zadania>()
        {
            new() {Id = 1, Title = "Вычисление функций с помощью рядов"},
            new() {Id = 2, Title = "Решение уравнений"},
            new() {Id = 3, Title = "Вычисление определенных интегралов"},
            new() {Id = 4, Title = "Интерполяция алгебраическими полиномами"},
            new() {Id = 5, Title = "Аппроксимация алгебраическими полиномами"},
            new() {Id = 6, Title = "Обращение матриц"},
            new() {Id = 7, Title = "Решение системы линейных уравнений итерационнами методами"},
            new() {Id = 8, Title = "Решение системы нелинейных уравнений"},
            new() {Id = 9, Title = "Вычисление интегралов методом Монте-Карло"}
        };
    }
}
