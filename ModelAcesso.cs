class Usuario
{
    private int id;
    private string nome;
    private List<Ambiente> ambientes;

    public int Id { get { return id; } set { id = value; } }
    public string Nome { get { return nome; } set { nome = value; } }
    public List<Ambiente> Ambientes { get { return ambientes; } set { ambientes = value; } }

    public Usuario(int id, string name)
    {
        Id = id;
        Nome = name;
        Ambientes = new List<Ambiente>();
    }

    public Usuario(int id)
    {
        Id = id;
        Nome = "";
        Ambientes = new List<Ambiente>();
    }

    public bool concederPermissao(Ambiente ambiente)
    {
        if(Ambientes.Count != 0)
        {
            foreach (Ambiente permitido in Ambientes)
            {
                if (permitido.Id == ambiente.Id)
                    return false;
            }
        }

        Ambientes.Add(ambiente);
        return true;
    }
    public bool revogarPermissao(Ambiente ambiente)
    {
        if(Ambientes.Count != 0)
        {
            foreach (Ambiente permitido in Ambientes)
            {
                if (permitido.Id == ambiente.Id)
                {
                    Ambientes.Remove(permitido);
                    return true;
                }
            }
        }

        return false;
    }
}

class Ambiente
{
    private int id;
    private string nome;
    private Queue<Log> logs;

    public int Id { get { return id; } set { id = value; }}
    public string Nome { get { return nome; } set { nome = value; } }
    public Queue<Log> Logs { get { return logs; } set { logs = value; } }

    public Ambiente(int id, string nome)
    {
        Id = id;
        Nome = nome;
        Logs = new Queue<Log>();
    }

    public Ambiente(int id)
    {
        Id = id;
        Nome = "";
        Logs = new Queue<Log>();
    }

    public void registrarLog(Log log)
    {
        Logs.Enqueue(log);
    }
    public void apagarLog()
    {
        Logs.Dequeue();
    }
}

class Log
{
    private DateTime dtAcesso;
    private Usuario usuario;
    private bool tipoAcesso;

    public DateTime DtAcesso { get { return dtAcesso;} set { dtAcesso = value; }}
    public Usuario Usuario { get { return usuario; } set { usuario = value; }}
    public bool TipoAcesso { get { return tipoAcesso;} set { tipoAcesso = value; }}

    public Log(Usuario usuario, bool acesso)
    {
        DtAcesso = DateTime.Now;
        Usuario = usuario;
        TipoAcesso = acesso;
    }
}