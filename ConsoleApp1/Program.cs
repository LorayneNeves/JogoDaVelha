using System;

class JogoDaVelha
{
	static char[,] tabuleiro = {
		{ ' ', ' ', ' ' },
		{ ' ', ' ', ' ' },
		{ ' ', ' ', ' ' }
	};

	static char maquina = 'X';
	static char adversario = 'O';

	static void Main(string[] args)
	{
		while (true)
		{
			Console.Clear();
			ExibirTabuleiro();
			if (VerificarVencedor(maquina))
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

			if (maquina == 'X')
			{
				var jogada = CalcularMelhorJogada();
				tabuleiro[jogada.Item1, jogada.Item2] = maquina;
			}
			else
			{
				Console.WriteLine("Vez do jogador O. Escolha uma posição (linha e coluna): ");
				int linha = int.Parse(Console.ReadLine());
				int coluna = int.Parse(Console.ReadLine());
				if (tabuleiro[linha, coluna] == ' ')
				{
					tabuleiro[linha, coluna] = adversario;
				}
			}

			maquina = (maquina == 'X') ? 'O' : 'X'; 
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
					if (i == 1 && j == 1)
						pontuacao[i, j] += 2;

					if ((i == 0 && j == 0) || (i == 0 && j == 2) || (i == 2 && j == 0) || (i == 2 && j == 2))
						pontuacao[i, j] += 1;

					if (LinhaTemAdversario(i, j) || ColunaTemAdversario(i, j) || DiagonalTemAdversario(i, j))
						pontuacao[i, j] -= 2;

					if (SimulaJogadaEVerificaVitoria(i, j, adversario))
						pontuacao[i, j] += 4;

					if (SimulaJogadaEVerificaVitoria(i, j, maquina))
						pontuacao[i, j] += 4;
				}
			}
		}

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
		tabuleiro[linha, coluna] = ' '; 
		return vitoria;
	}

	static bool VerificarVencedor(char jogador)
	{
		for (int i = 0; i < 3; i++)
		{
			if (tabuleiro[i, 0] == jogador && tabuleiro[i, 1] == jogador && tabuleiro[i, 2] == jogador)
				return true;
			if (tabuleiro[0, i] == jogador && tabuleiro[1, i] == jogador && tabuleiro[2, i] == jogador)
				return true;
		}

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
