using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramNumericalMet.Models
{
    public class Fourier
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double SumEven { get; set; }
        public double SumOdd { get; set; }
    }
}
