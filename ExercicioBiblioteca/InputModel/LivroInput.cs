using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExercicioBiblioteca.InputModel
{
    public class LivroInput
    {
        public string Descricao { get; set; }
        public int ISBN { get; set; }
        public int AnoLancamento { get; set; }
        public int CodigoAutor { get; set; }
        public string CodigoEditora { get; set; }

    }
}
