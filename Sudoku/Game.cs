using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
	class Game
	{
		public Board board;

		public int Mistakes { get; private set; }

		public void IncrementMistakes()
		{
			Mistakes++;
		}

		public void ResetMistakes()
		{
			Mistakes = 0;
		}

		public const int maxMistakes = 5;

	}
}
