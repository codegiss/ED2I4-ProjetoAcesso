// dotnet add package MySqlConnector
// dotnet add package MySql.Data

int op = -1, idAmbiente, idUsuario, qtdAmbiente = 0, qtdUsuario = 0;
string nomeAmbiente, nomeUsuario;
Cadastro cadastro = new Cadastro();
Index id = new Index();

cadastro.download();

do
{
    do
    {
        try
        {
            Console.WriteLine("Selecione uma opção:");
            Console.WriteLine("0. Sair");
            Console.WriteLine("1. Cadastrar ambiente");
            Console.WriteLine("2. Consultar ambiente");
            Console.WriteLine("3. Excluir ambiente");
            Console.WriteLine("4. Cadastrar usuario");
            Console.WriteLine("5. Consultar usuario");
            Console.WriteLine("6. Excluir usuario");
            Console.WriteLine("7. Conceder permissão de acesso ao usuario");
            Console.WriteLine("8. Revogar permissão de acesso ao usuario");
            Console.WriteLine("9. Registrar acesso");
            Console.WriteLine("10. Consultar logs de acesso");
            Console.Write("Opção escolhida: ");
            op = int.Parse(Console.ReadLine());
        }
        catch (Exception ex)
        {
            op = -1;
            Console.WriteLine(ex.Message);
        }
    }
    while (op < 0 || op > 10);

    switch(op)
    {
        case 1:
            nomeAmbiente = "";

            do
            {
                Console.Write("Nome do novo ambiente: ");
                nomeAmbiente = Console.ReadLine();
            }
            while(nomeAmbiente == "");

            idAmbiente = ++qtdAmbiente;
            Ambiente novoAmbiente = new Ambiente(idAmbiente, nomeAmbiente);
            cadastro.adicionarAmbiente(novoAmbiente);

            Console.WriteLine("Ambiente adicionado.");
            break;
        case 2:
            idAmbiente = id.askId("ambiente");

            novoAmbiente = new Ambiente(idAmbiente);
            Ambiente pesquisaAmbiente = cadastro.pesquisarAmbiente(novoAmbiente);

            Console.WriteLine(pesquisaAmbiente.Id != -1 ?
            "Nome: "+ pesquisaAmbiente.Nome + 
            "\nQtd de logs armazenados: " + pesquisaAmbiente.Logs.Count
             : "Verifique o id e tente novamente.");
            break;
        case 3:
            idAmbiente = id.askId("ambiente");

            Ambiente deletarAmbiente = new Ambiente(idAmbiente);
            Console.WriteLine(cadastro.removerAmbiente(deletarAmbiente) ?
            "Ambiente removido." : "Ambiente não pode ser removido.");
            break;
        case 4:
            nomeUsuario = "";
            do
            {
                Console.Write("Nome do novo usuário: ");
                nomeUsuario = Console.ReadLine();
            }
            while (nomeUsuario == "");

            idUsuario = ++qtdUsuario;
            Usuario novoUsuario = new Usuario(idUsuario, nomeUsuario);
            cadastro.adicionarUsuario(novoUsuario);

            Console.WriteLine("Usuário adicionado.");
            break;
        case 5:
            idUsuario = id.askId("usuário");

            novoUsuario = new Usuario(idUsuario);
            Usuario pesquisaUsuario = cadastro.pesquisarUsuario(novoUsuario);

            Console.WriteLine(pesquisaUsuario.Id != -1 ?
            "Nome: " + pesquisaUsuario.Nome +
            "\nQtd de ambientes permitidos: " + pesquisaUsuario.Ambientes.Count
             : "Verifique o id e tente novamente.");
            break;
        case 6:
            idUsuario = id.askId("usuário");

            Usuario deletarUsuario = new Usuario(idUsuario);
            Console.WriteLine(cadastro.removerUsuario(deletarUsuario)?
            "Usuário removido." : "Usuário não existe e não pode ser removido.");
            break;
        case 7:
            idUsuario = id.askId("usuário");
            Usuario permitirUsuario = cadastro.pesquisarUsuario(new Usuario(idUsuario));
            Console.WriteLine("id usuario: "+permitirUsuario.Id);

            if(permitirUsuario.Id != -1)
            {
                idAmbiente = id.askId("ambiente");
                Ambiente permitirAmbiente = cadastro.pesquisarAmbiente(new Ambiente(idAmbiente));
                Console.WriteLine("id ambiente: "+permitirAmbiente.Id);

                if(permitirAmbiente.Id != -1)
                {
                    bool permissao = cadastro.permitirAmbiente(permitirUsuario, permitirAmbiente);
                    Console.WriteLine(permissao?
                    "Acesso ao ambiente "+ permitirAmbiente.Nome +" permitido ao usuário " + permitirUsuario.Nome
                     : "O usuário já possui acesso ao ambiente");
                }
                else
                {
                    Console.WriteLine("Ambiente inexistente. Verifique o id");
                }
            }
            else
            {
                Console.WriteLine("Usuário inexistente. Verifique o id");
            }
            break;
        case 8:
            idUsuario = id.askId("usuário");
            Usuario revogarUsuario = cadastro.pesquisarUsuario(new Usuario(idUsuario));

            if (revogarUsuario.Id != -1)
            {
                idAmbiente = id.askId("ambiente");
                Ambiente revogarAmbiente = cadastro.pesquisarAmbiente(new Ambiente(idAmbiente));

                if (revogarAmbiente.Id != -1)
                {
                    Console.WriteLine(cadastro.revogarAmbiente(revogarUsuario, revogarAmbiente) ?
                    "Acesso ao ambiente " + revogarAmbiente.Nome + " não é mais permitido ao usuário " + revogarUsuario.Nome
                     : "O usuário já não possuía acesso ao ambiente");
                }
                else
                {
                    Console.WriteLine("Ambiente inexistente. Verifique o id");
                }
            }
            else
            {
                Console.WriteLine("Usuário inexistente. Verifique o id");
            }
            break;
        case 9:
            idUsuario = id.askId("usuário");
            idAmbiente = id.askId("ambiente");

            Usuario acessarUsuario = cadastro.pesquisarUsuario(new Usuario(idUsuario));
            Ambiente acessarAmbiente = cadastro.pesquisarAmbiente(new Ambiente(idAmbiente));

            if(acessarUsuario.Id == -1 || acessarAmbiente.Id == -1)
            {
                Console.WriteLine("Verifique os ids e tente novamente");
            }
            else
            {
                Console.Write(cadastro.tentarAcesso(acessarUsuario, acessarAmbiente)?
                "Acesso realizado " : "Acesso negado ");
                Console.WriteLine("e log registrado no ambiente.");
            }
            break;
        case 10:
            idAmbiente = id.askId("ambiente");

            Ambiente verLogs = cadastro.pesquisarAmbiente(new Ambiente(idAmbiente));

            if(verLogs.Id == -1)
                Console.WriteLine("Verifique o id do ambiente e tente de novo.");
            else
            {
                int opcao = -1;

                do
                {
                    try
                    {
                        Console.WriteLine("Selecione uma opção:");
                        Console.WriteLine("0. Logs autorizados");
                        Console.WriteLine("1. Logs negados");
                        Console.WriteLine("2. Ver todos os logs");
                        Console.Write("Opção escolhida: ");
                        opcao = int.Parse(Console.ReadLine());
                    }
                    catch (Exception ex)
                    {
                        opcao = -1;
                        Console.WriteLine(ex.Message);
                    }
                }
                while (opcao < 0 || opcao > 2);

                string logs = cadastro.consultarLogs(opcao, verLogs);

                Console.WriteLine(logs == ""?
                "Não há logs registrados para esse ambiente." : logs);
            }
            break;
    }
}
while (op != 0);