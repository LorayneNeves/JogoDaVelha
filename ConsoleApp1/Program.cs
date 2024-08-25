using System;

class JogoDaVelha
{
	static char[,] tabuleiro = {
		{ ' ', ' ', ' ' },
		{ ' ', ' ', ' ' },
		{ ' ', ' ', ' ' }
	};

	static char jogador = 'X';
	static char adversario = 'O';

	static void Main(string[] args)
	{
		while (true)
		{
			Console.Clear();
			ExibirTabuleiro();
			if (VerificarVencedor(jogador))
			{
				Console.WriteLine("Jogador X venceu!");
				break;
			}
			else if (VerificarVencedor(adversario))
			{
				Console.WriteLine("Jogador O venceu!");
				break;
			}
			else if (VerificarEmpate())
			{
				Console.WriteLine("Empate!");
				break;
			}

			if (jogador == 'X')
			{
				// Estratégia de Pontuação (4a)
				var jogada = CalcularMelhorJogada();
				tabuleiro[jogada.Item1, jogada.Item2] = jogador;
			}
			else
			{
				// Para simplificar, o adversário joga manualmente
				Console.WriteLine("Vez do jogador O. Escolha uma posição (linha e coluna): ");
				int linha = int.Parse(Console.ReadLine());
				int coluna = int.Parse(Console.ReadLine());
				if (tabuleiro[linha, coluna] == ' ')
				{
					tabuleiro[linha, coluna] = adversario;
				}
			}

			jogador = (jogador == 'X') ? 'O' : 'X'; // Alterna jogadores
		}
	}

	static void ExibirTabuleiro()
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				Console.Write(tabuleiro[i, j]);
				if (j < 2) Console.Write("|");
			}
			Console.WriteLine();
			if (i < 2) Console.WriteLine("-----");
		}
	}

	static Tuple<int, int> CalcularMelhorJogada()
	{
		int[,] pontuacao = new int[3, 3];

		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (tabuleiro[i, j] == ' ')
				{
					// Mais 2 pontos se for a posição central
					if (i == 1 && j == 1)
						pontuacao[i, j] += 2;

					// Mais 1 ponto se for um dos cantos
					if ((i == 0 && j == 0) || (i == 0 && j == 2) || (i == 2 && j == 0) || (i == 2 && j == 2))
						pontuacao[i, j] += 1;

					// Menos 2 pontos se o adversário estiver na mesma linha, coluna ou diagonal
					if (LinhaTemAdversario(i, j) || ColunaTemAdversario(i, j) || DiagonalTemAdversario(i, j))
						pontuacao[i, j] -= 2;

					// Mais 4 pontos se bloquear a vitória do adversário
					if (SimulaJogadaEVerificaVitoria(i, j, adversario))
						pontuacao[i, j] += 4;

					// Mais 4 pontos se levar à vitória
					if (SimulaJogadaEVerificaVitoria(i, j, jogador))
						pontuacao[i, j] += 4;
				}
			}
		}

		// Escolher a posição com a maior pontuação
		int melhorLinha = 0, melhorColuna = 0, maiorPontuacao = int.MinValue;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (pontuacao[i, j] > maiorPontuacao)
				{
					maiorPontuacao = pontuacao[i, j];
					melhorLinha = i;
					melhorColuna = j;
				}
			}
		}
		return Tuple.Create(melhorLinha, melhorColuna);
	}

	static bool LinhaTemAdversario(int linha, int coluna)
	{
		for (int j = 0; j < 3; j++)
		{
			if (tabuleiro[linha, j] == adversario)
				return true;
		}
		return false;
	}

	static bool ColunaTemAdversario(int linha, int coluna)
	{
		for (int i = 0; i < 3; i++)
		{
			if (tabuleiro[i, coluna] == adversario)
				return true;
		}
		return false;
	}

	static bool DiagonalTemAdversario(int linha, int coluna)
	{
		if (linha == coluna)
		{
			for (int i = 0; i < 3; i++)
			{
				if (tabuleiro[i, i] == adversario)
					return true;
			}
		}
		if (linha + coluna == 2)
		{
			for (int i = 0; i < 3; i++)
			{
				if (tabuleiro[i, 2 - i] == adversario)
					return true;
			}
		}
		return false;
	}

	static bool SimulaJogadaEVerificaVitoria(int linha, int coluna, char jogador)
	{
		tabuleiro[linha, coluna] = jogador;
		bool vitoria = VerificarVencedor(jogador);
		tabuleiro[linha, coluna] = ' '; // Desfazer jogada simulada
		return vitoria;
	}

	static bool VerificarVencedor(char jogador)
	{
		// Verificar linhas e colunas
		for (int i = 0; i < 3; i++)
		{
			if (tabuleiro[i, 0] == jogador && tabuleiro[i, 1] == jogador && tabuleiro[i, 2] == jogador)
				return true;
			if (tabuleiro[0, i] == jogador && tabuleiro[1, i] == jogador && tabuleiro[2, i] == jogador)
				return true;
		}
		// Verificar diagonais
		if (tabuleiro[0, 0] == jogador && tabuleiro[1, 1] == jogador && tabuleiro[2, 2] == jogador)
			return true;
		if (tabuleiro[0, 2] == jogador && tabuleiro[1, 1] == jogador && tabuleiro[2, 0] == jogador)
			return true;
		return false;
	}

	static bool VerificarEmpate()
	{
		foreach (var posicao in tabuleiro)
		{
			if (posicao == ' ')
				return false;
		}
		return true;
	}
}
