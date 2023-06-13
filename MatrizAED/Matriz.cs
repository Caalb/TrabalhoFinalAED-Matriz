using System;

namespace MatrizAED
{
    public class Celula
    {
        public int Valor { get; set; }
        public Celula Superior { get; set; }
        public Celula Inferior { get; set; }
        public Celula Esquerda { get; set; }
        public Celula Direita { get; set; }

        public Celula(int valor)
        {
            Valor = valor;
            Superior = null;
            Inferior = null;
            Esquerda = null;
            Direita = null;
        }
    }

    public class Matriz
    {
        private Celula inicio;
        private int linha, coluna;

        public Matriz() : this(3, 3) { }

        public Matriz(int linhas, int colunas)
        {
            linha = linhas;
            coluna = colunas;
            CriarMatriz(linhas, colunas);
        }

        private void CriarMatriz(int linhas, int colunas)
        {
            Celula celulaAtual = null;
            Celula celulaAnterior = null;

            for (int i = 0; i < linhas; i++)
            {
                celulaAnterior = null;
                for (int j = 0; j < colunas; j++)
                {
                    celulaAtual = new Celula(0);

                    if (j > 0)
                    {
                        celulaAnterior.Direita = celulaAtual;
                        celulaAtual.Esquerda = celulaAnterior;
                    }

                    if (i > 0)
                    {
                        Celula celulaSuperior = ObterCelula(i - 1, j);
                        celulaSuperior.Inferior = celulaAtual;
                        celulaAtual.Superior = celulaSuperior;
                    }

                    if (i == 0 && j == 0)
                    {
                        inicio = celulaAtual;
                    }

                    celulaAnterior = celulaAtual;
                }
            }
        }

        private Celula ObterCelula(int linha, int coluna)
        {
            Celula celulaAtual = inicio;
            for (int i = 0; i < linha; i++)
            {
                celulaAtual = celulaAtual.Inferior;
            }

            for (int j = 0; j < coluna; j++)
            {
                celulaAtual = celulaAtual.Direita;
            }

            return celulaAtual;
        }

        public void Mostrar()
        {
            for (int i = 0; i < linha; i++)
            {
                for (int j = 0; j < coluna; j++)
                {
                    Celula celulaAtual = ObterCelula(i, j);
                    Console.Write(celulaAtual.Valor + "\t");
                }
                Console.WriteLine();
            }
        }

        public int[] CalcularDiagonal()
        {
            int[] diagonal = new int[Math.Min(linha, coluna)];

            for (int i = 0; i < diagonal.Length; i++)
            {
                diagonal[i] = ObterCelula(i, i).Valor;
            }

            return diagonal;
        }

        public static Matriz Somar(Matriz a, Matriz b)
        {
            if (a.linha != b.linha || a.coluna != b.coluna)
            {
                throw new ArgumentException("As matrizes devem ter o mesmo tamanho para serem somadas.");
            }

            Matriz resultado = new Matriz(a.linha, a.coluna);

            for (int i = 0; i < a.linha; i++)
            {
                for (int j = 0; j < a.coluna; j++)
                {
                    resultado.ObterCelula(i, j).Valor = a.ObterCelula(i, j).Valor + b.ObterCelula(i, j).Valor;
                }
            }

            return resultado;
        }

        public static Matriz Multiplicar(Matriz a, Matriz b)
        {
            if (a.coluna != b.linha)
            {
                throw new ArgumentException("O número de colunas da primeira matriz deve ser igual ao número de linhas da segunda matriz.");
            }

            Matriz resultado = new Matriz(a.linha, b.coluna);

            for (int i = 0; i < a.linha; i++)
            {
                for (int j = 0; j < b.coluna; j++)
                {
                    int soma = 0;

                    for (int k = 0; k < a.coluna; k++)
                    {
                        soma += a.ObterCelula(i, k).Valor * b.ObterCelula(k, j).Valor;
                    }

                    resultado.ObterCelula(i, j).Valor = soma;
                }
            }

            return resultado;
        }

        public static void PreencherComValoresAleatorios(Matriz matriz, int min, int max)
        {
            Random random = new Random();

            for (int i = 0; i < matriz.linha; i++)
            {
                for (int j = 0; j < matriz.coluna; j++)
                {
                    matriz.ObterCelula(i, j).Valor = random.Next(min, max);
                }
            }
        }

        private void TrocarLinhas(int linha1, int linha2)
        {
            for (int j = 0; j < coluna; j++)
            {
                double temp = ObterCelula(linha1, j).Valor;
                ObterCelula(linha1, j).Valor = ObterCelula(linha2, j).Valor;
                ObterCelula(linha2, j).Valor = (int)temp;
            }
        }

        public bool VerificarIdempotente()
        {
            for (int i = 0; i < linha; i++)
            {
                for (int j = 0; j < coluna; j++)
                {
                    int valor = ObterCelula(i, j).Valor;
                    int valorQuadrado = ObterCelula(i, j).Valor * ObterCelula(i, j).Valor;

                    if (valor != valorQuadrado)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public (Matriz, Matriz) DecomposicaoQR()
        {
            Matriz q = new Matriz(linha, coluna);
            Matriz r = new Matriz(coluna, coluna);

            Matriz a = new Matriz(linha, coluna);

            for (int i = 0; i < linha; i++)
            {
                for (int j = 0; j < coluna; j++)
                {
                    a.ObterCelula(i, j).Valor = ObterCelula(i, j).Valor;
                }
            }

            for (int k = 0; k < coluna; k++)
            {
                double norma = 0;

                for (int i = k; i < linha; i++)
                {
                    norma += a.ObterCelula(i, k).Valor * a.ObterCelula(i, k).Valor;
                }

                double raizNorma = Math.Sqrt(norma);
                r.ObterCelula(k, k).Valor = (int)raizNorma;

                for (int i = k; i < linha; i++)
                {
                    q.ObterCelula(i, k).Valor = (int)(a.ObterCelula(i, k).Valor / raizNorma);
                }

                for (int j = k + 1; j < coluna; j++)
                {
                    double soma = 0;

                    for (int i = k; i < linha; i++)
                    {
                        soma += q.ObterCelula(i, k).Valor * a.ObterCelula(i, j).Valor;
                    }

                    r.ObterCelula(k, j).Valor = (int)soma;

                    for (int i = k; i < linha; i++)
                    {
                        a.ObterCelula(i, j).Valor = (int)(a.ObterCelula(i, j).Valor - soma * q.ObterCelula(i, k).Valor);
                    }
                }
            }

            return (q, r);
        }

        public Matriz DecomposicaoCholesky()
        {
            if (linha != coluna)
            {
                throw new ArgumentException("A matriz deve ser quadrada para a decomposição de Cholesky.");
            }

            Matriz l = new Matriz(linha, coluna);

            for (int j = 0; j < coluna; j++)
            {
                double soma = 0;
                for (int k = 0; k < j; k++)
                {
                    soma += l.ObterCelula(j, k).Valor * l.ObterCelula(j, k).Valor;
                }
                double diagonal = ObterCelula(j, j).Valor - soma;
                if (diagonal <= 0)
                {
                    throw new ArithmeticException("A matriz não é positiva definida.");
                }
                l.ObterCelula(j, j).Valor = (int)Math.Sqrt(diagonal);

                for (int i = j + 1; i < linha; i++)
                {
                    soma = 0;
                    for (int k = 0; k < j; k++)
                    {
                        soma += l.ObterCelula(i, k).Valor * l.ObterCelula(j, k).Valor;
                    }
                    l.ObterCelula(i, j).Valor = (int)((ObterCelula(i, j).Valor - soma) / l.ObterCelula(j, j).Valor);
                }
            }

            return l;
        }

        public static void Teste()
        {
            Matriz matriz = new Matriz(3, 3);
            PreencherComValoresAleatorios(matriz, 0, 10);

            Console.WriteLine("Matriz original:");
            matriz.Mostrar();

            Console.WriteLine("\nDiagonal:");
            int[] diagonal = matriz.CalcularDiagonal();
            foreach (int valor in diagonal)
            {
                Console.Write(valor + "\t");
            }
            Console.WriteLine();

            Matriz matrizA = new Matriz(3, 3);
            PreencherComValoresAleatorios(matrizA, 0, 10);
            Matriz matrizB = new Matriz(3, 3);
            PreencherComValoresAleatorios(matrizB, 0, 10);

            Console.WriteLine("\nMatriz A:");
            matrizA.Mostrar();
            Console.WriteLine("\nMatriz B:");
            matrizB.Mostrar();

            Console.WriteLine("\nSoma de matrizes A e B:");
            Matriz soma = Somar(matrizA, matrizB);
            soma.Mostrar();

            Console.WriteLine("\nMultiplicação de matrizes A e B:");
            Matriz multiplicacao = Multiplicar(matrizA, matrizB);
            multiplicacao.Mostrar();
            Console.WriteLine();


            Matriz matriz1 = new Matriz(2, 2);
            PreencherComValoresAleatorios(matriz1, 0, 10);

            Matriz matriz2 = new Matriz(3, 3);
            PreencherComValoresAleatorios(matriz2, 0, 10);

            Console.WriteLine("Matriz 1:");
            matriz1.Mostrar();
            Console.WriteLine();

            Console.WriteLine("Matriz 2:");
            matriz2.Mostrar();
            Console.WriteLine();

            Matriz matrizTeste = new Matriz(3, 3);
            PreencherComValoresAleatorios(matrizTeste, 0, 10);

            Console.WriteLine("\nMatriz original:");
            matrizTeste.Mostrar();

            Console.WriteLine("\nApós trocar as linhas 0 e 1:");
            matrizTeste.TrocarLinhas(0, 1);
            matrizTeste.Mostrar();

            Matriz matriz6 = new Matriz(3, 3);
            PreencherComValoresAleatorios(matriz6, 0, 10);
            Console.WriteLine("\nMatriz:");
            matriz6.Mostrar();
            Console.WriteLine();
            Console.WriteLine("A matriz acima é idempotente? " + matriz6.VerificarIdempotente());
            Console.WriteLine();

            Console.WriteLine("Decomposição QR:");
            var (q, r) = matriz2.DecomposicaoQR();
            matriz2.Mostrar();
            Console.WriteLine();

            Console.WriteLine("Matriz Q:");
            q.Mostrar();
            Console.WriteLine();

            Console.WriteLine("Matriz R:");
            r.Mostrar();

            Console.WriteLine("Decomposição de Cholesky:");
            Console.WriteLine();
            Matriz matriz8 = new Matriz(2, 2);
            PreencherComValoresAleatorios(matriz8, 1, 10);
            Console.WriteLine("Matriz original");
            matriz8.Mostrar();
            Console.WriteLine();

            var l = matriz8.DecomposicaoCholesky();

            Console.WriteLine("\nMatriz L:");
            l.Mostrar();
        }
    }
}