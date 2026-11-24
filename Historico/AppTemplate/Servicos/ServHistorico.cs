namespace HistoricoApp.Servicos
{
    public class ServHistorico
    {
        private readonly DataContext _dataContext;

        public ServHistorico(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void RegistrarEvento(RegistroHistorico novoRegistro)
        {
            ValidarRegistro(novoRegistro);

            novoRegistro.Id = Guid.NewGuid();
            novoRegistro.DataEvento = DateTime.Now;

            _dataContext.Add(novoRegistro);
            _dataContext.SaveChanges();
        }

        public void ValidarRegistro(RegistroHistorico registro)
        {
            if (registro.CodigoEmprestimo == Guid.Empty)
            {
                throw new Exception("Falta informar o código do empréstimo.");
            }

            if (registro.CodigoLivro == Guid.Empty)
            {
                throw new Exception("Falta informar o código do livro.");
            }

            if (registro.CodigoLeitor == Guid.Empty)
            {
                throw new Exception("Falta informar o código do leitor.");
            }

            if (!Enum.IsDefined(registro.TipoEvento))
            {
                throw new Exception("Tipo de evento inválido.");
            }
        }

        public List<RegistroHistorico> ListarHistorico()
        {
            return _dataContext.RegistrosHistorico
                .OrderByDescending(p => p.DataEvento)
                .ToList();
        }

        public List<RegistroHistorico> ListarPorLeitor(Guid codigoLeitor)
        {
            return _dataContext.RegistrosHistorico
                .Where(p => p.CodigoLeitor == codigoLeitor)
                .OrderByDescending(p => p.DataEvento)
                .ToList();
        }

        public List<RegistroHistorico> ListarPorLivro(Guid codigoLivro)
        {
            return _dataContext.RegistrosHistorico
                .Where(p => p.CodigoLivro == codigoLivro)
                .OrderByDescending(p => p.DataEvento)
                .ToList();
        }

        public List<RegistroHistorico> ListarPorEmprestimo(Guid codigoEmprestimo)
        {
            return _dataContext.RegistrosHistorico
                .Where(p => p.CodigoEmprestimo == codigoEmprestimo)
                .OrderBy(p => p.DataEvento)
                .ToList();
        }
    }
}
