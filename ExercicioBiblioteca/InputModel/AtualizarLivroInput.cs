﻿namespace ExercicioBiblioteca.InputModel
{
    public class AtualizarLivroInput
    {
        public int Codigo { get; set; }
        public string  Descricao { get; set; }
        public int ISBN { get; set; }
        public int AnoLancamento { get; set; }
    }
}
