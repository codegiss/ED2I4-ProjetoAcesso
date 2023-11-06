using MySql.Data.MySqlClient;

class Cadastro
{
    private List<Usuario> usuarios;
    private List<Ambiente> ambientes;

    public List<Usuario> Usuarios { get { return usuarios; } set { usuarios = value; } }
    public List<Ambiente> Ambientes { get { return ambientes; } set { ambientes = value; } }

    public Cadastro()
    {
        Usuarios = new List<Usuario>();
        Ambientes = new List<Ambiente>();
    }

    public void adicionarUsuario(Usuario usuario)
    {
        Usuarios.Add(usuario);
    }

    public bool removerUsuario(Usuario usuario)
    {
        int index = Usuarios.FindIndex(e => e.Id == usuario.Id);

        if(index == -1) return false;

        Usuarios.RemoveAt(index);
        return true;
    }

    public Usuario pesquisarUsuario(Usuario usuario)
    {
        int index = Usuarios.FindIndex(e => e.Id == usuario.Id);

        if(index == -1) return new Usuario(-1);
        return Usuarios[index];
    }

    public void adicionarAmbiente(Ambiente ambiente)
    {
        Ambientes.Add(ambiente);
    }

    public bool removerAmbiente(Ambiente ambiente)
    {
        int index = Ambientes.FindIndex(e => e.Id == ambiente.Id);

        if (index == -1) return false;

        foreach(Usuario usuarios in Usuarios)
        {
            usuarios.revogarPermissao(ambiente);
        }

        Ambientes.RemoveAt(index);
        return true;
    }

    public Ambiente pesquisarAmbiente(Ambiente ambiente)
    {
        int index = Ambientes.FindIndex(e => e.Id == ambiente.Id);

        if (index == -1) return new Ambiente(-1);
        return Ambientes[index];
    }

    public bool permitirAmbiente(Usuario usuario, Ambiente ambiente)
    {
        int indexUsuario = pesquisarUsuario(usuario).Id;
        Console.WriteLine(indexUsuario);

        if (indexUsuario == -1) return false;
        Console.WriteLine(indexUsuario);

        Usuarios[indexUsuario-1].concederPermissao(ambiente);
        return true;
    }
    
    public bool revogarAmbiente(Usuario usuario, Ambiente ambiente)
    {
        int indexUsuario = pesquisarAmbiente(ambiente).Id;
        if (indexUsuario == -1) return false;

        return Usuarios[indexUsuario-1].revogarPermissao(ambiente);
    }

    public bool tentarAcesso(Usuario usuario, Ambiente ambiente)
    {
        bool permissao = false;

        foreach(Ambiente permitido in usuario.Ambientes)
        {
            if(permitido.Id == ambiente.Id)
            {
                permissao = true;
            }
        }

        Log login = new Log(usuario, permissao);
        int id = pesquisarAmbiente(ambiente).Id;

        if(Ambientes[id-1].Logs.Count == 100)
        {
            while(Ambientes[id-1].Logs.Count > 99)
                Ambientes[id - 1].apagarLog();
        }
        
        Ambientes[id-1].registrarLog(login);

        return permissao;
    }

    public string consultarLogs(int opcao, Ambiente ambiente)
    {
        string pesquisaLogs = "";

        switch(opcao)
        {
            case 0:
                foreach(Log log in ambiente.Logs)
                {
                    if(log.TipoAcesso == true)
                    {
                        pesquisaLogs += log.DtAcesso.ToString("dd/MM/yyyy HH:mm:ss") + " - ";
                        pesquisaLogs += log.Usuario.Id + "_" + log.Usuario.Nome + " - ";
                        pesquisaLogs += log.TipoAcesso == true? "Access allowed" : "Access denied";
                        pesquisaLogs += "\n";
                    }
                }
                break;
            case 1:
                foreach (Log log in ambiente.Logs)
                {
                    if (log.TipoAcesso == false)
                    {
                        pesquisaLogs += log.DtAcesso.ToString("dd/MM/yyyy HH:mm:ss") + " - ";
                        pesquisaLogs += log.Usuario.Id + "_" + log.Usuario.Nome + " - ";
                        pesquisaLogs += log.TipoAcesso == true ? "Access allowed" : "Access denied";
                        pesquisaLogs += "\n";
                    }
                }
                break;
            case 2:
                foreach (Log log in ambiente.Logs)
                {
                    pesquisaLogs += log.DtAcesso.ToString("dd/MM/yyyy HH:mm:ss") + " - ";
                    pesquisaLogs += log.Usuario.Id + "_" + log.Usuario.Nome + " - ";
                    pesquisaLogs += log.TipoAcesso == true ? "Access allowed" : "Access denied";
                    pesquisaLogs += "\n";
                }
                break;
        }

        return pesquisaLogs;
    }

    public void upload()
    {

    }

    public void download()
    {
        string connStr = "server=localhost;user=root;port=3306;password=password;";
        MySqlConnection conn = new MySqlConnection(connStr);

        try
        {
            conn.Open();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Conexão falhou.");
            Console.WriteLine(ex.Message);
        }
        Console.WriteLine("Conexão estabelecida.");
        conn.Close();
    }
}

class Index
{
    private int id;
    public int Id { get { return id; } set { id = value; } }

    public int askId(string tipo)
    {
        Id = 0;

        do
        {
            try
            {
                Console.Write("Id do " + tipo + ": ");
                Id = int.Parse(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Id = 0;
                Console.WriteLine(ex.Message);
            }
        }
        while (Id == 0);

        return Id;
    }
}