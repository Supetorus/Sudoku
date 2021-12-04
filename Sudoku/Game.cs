using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
	[Serializable]
	class Game
	{
		public Board board;

		public int Mistakes { get; private set; }

		public int HintNum { get; set; } = 3;
		public int TotalHints = 3;
		public int Time { get; set; } = 0;

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
