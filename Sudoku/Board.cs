
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Sudoku
{
	[Serializable]
	class Board
	{
		const int size = 9;

		int difficulty = 1;

		int[,] solved = new int[size, size]; // Full board with all numbers
		int[,] unsolved = new int[size, size]; // The starting board with empty spaces
		int[,] current = new int[size, size]; // The starting board plus numbers user has entered

		int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

		int[] indexes = { 
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 
			20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 
			30, 31, 32, 33, 34, 35, 36, 37, 38, 39,
			40, 41, 42, 43, 44, 45, 46, 47, 48, 49,
			50, 51, 52, 53, 54, 55, 56, 57, 58, 59,
			60, 61, 62, 63, 64, 65, 66, 67, 68, 69,
			70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80 };

		[NonSerialized] Random r = new Random();

		public Board(int difficulty)
		{
			this.difficulty = difficulty;
		}

		/// <summary>
		/// Generates a fresh board.
		/// </summary>
		public void Generate()
		{
			while (!FillGrid())
			{
				ClearGrid(solved);
			}

			do
			{
				CopyGrid(solved, unsolved);
				RemoveNums();
			} while (!SolveGrid() && !CompareGrids(solved, current));

			CopyGrid(unsolved, current);
		}

		/// <summary>
		/// Returns the number in the current board in the given location.
		/// </summary>
		/// <param name="x">Column, 0 based</param>
		/// <param name="y">Row, 0 based</param>
		/// <returns></returns>
		public int GetNum(int x, int y)
		{
			return current[x, y];
		}

		public int GetCorrectNum(int x, int y)
		{
			return solved[x, y];
		}

		/// <summary>
		/// Returns the number in the given position of the initial, or unsolved board.
		/// </summary>
		/// <param name="x">Column, 0 based</param>
		/// <param name="y">Row, 0 based</param>
		/// <returns></returns>
		public int GetUnsolvedNum(int x, int y)
		{ return unsolved[x, y];}

		void ClearGrid(int[,] grid)
		{
			for (int x = 0; x < size; ++x)
			{
				for (int y = 0; y < size; ++y)
				{
					grid[x, y] = 0;
				}
			}
		}

		// Returns true if the given grids are identical.
		bool CompareGrids(int[,] g1, int[,] g2)
		{
			for (int x = 0; x < size; ++x)
			{
				for (int y = 0; y < size; ++y)
				{
					if (g1[x, y] != g2[x, y])
					{
						return false;
					}
				}
			}

			return true;
		}

		void CopyGrid(int[,] g1, int[,] g2)
		{
			for (int x = 0; x < size; ++x)
			{
				for (int y = 0; y < size; ++y)
				{
					g2[x, y] = g1[x, y];
				}
			}
		}

		bool FillGrid()
		{
			for (int x = 0; x < 9; ++x)
			{
				for (int y = 0; y < 9; ++y)
				{
					bool flag = false;
					nums = nums.OrderBy(x => r.Next()).ToArray();

					foreach (int i in nums)
					{
						if (CheckSafety(x, y, i, solved))
						{
							solved[x, y] = i;
							flag = true;
							break;
						}
					}

					if (!flag)
					{
						return false;
					}
				}
			}

			return true;
		}

		void RemoveNums()
		{
			indexes = indexes.OrderBy(x => r.Next()).ToArray();

			for (int i = 0; i < difficulty * 21; ++i)
			{
				unsolved[indexes[i] / 9, indexes[i] % 9] = 0;
			}
		}

		bool SolveGrid()
		{
			CopyGrid(unsolved, current);

			for (int x = 0; x < size; ++x)
			{
				for (int y = 0; y < size; ++y)
				{
					if (unsolved[x, y] == 0)
					{
						bool flag = false;

						for (int i = 1; i < 10; ++i)
						{
							if (CheckSafety(x, y, i, current))
							{
								current[x, y] = i;
								flag = true;
							}
						}

						if (!flag)
						{
							return false;
						}
					}
				}
			}

			return true;
		}

		public bool CheckSafety(int x, int y, int i)
		{
			return CheckSafety(x, y, i, current);
		}

		/// <summary>
		/// Checks whether i is viable for the position. Used for generating the board.
		/// </summary>
		/// <param name="x">Column, 0 based</param>
		/// <param name="y">Row, 0 based</param>
		/// <param name="i">The number to be checked</param>
		/// <param name="grid">The grid to check in</param>
		/// <returns></returns>
		private bool CheckSafety(int x, int y, int i, int[,] grid)
		{
			//Check Row
			for (int gx = 0; gx < size; ++gx)
			{
				if (grid[gx, y] == i)
				{
					return false;
				}
			}

			//Check Column
			for (int gy = 0; gy < size; ++gy)
			{
				if (grid[x, gy] == i)
				{
					return false;
				}
			}

			//Check Box
			for (int bx = x - (x % 3); bx < x - (x % 3) + 3; ++bx)
			{
				for (int by = y - (y % 3); by < y - (y % 3) + 3; ++by)
				{
					if (grid[bx, by] == i)
					{
						return false;
					}
				}
			}

			return true;
		}

		public bool CheckNum(int x, int y, int i)
		{
			return i == solved[x, y];
		}

		public void SetNum(int x, int y, int i)
		{
			current[x, y] = i;
		}

		public void ResetBoard()
		{
			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					current[i, j] = unsolved[i, j];
				}
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			for (int y = 0; y < size; ++y)
			{
				for (int x = 0; x < size; ++x)
				{
					sb.Append(" ");
					sb.Append(current[x, y] == 0 ? " " : current[x, y]);
					sb.Append(" ");
					sb.Append(x == 8 ? "" : "|");
				}

				sb.AppendLine(y == 8 ? "" : "\n-----------------------------------");
			}

			return sb.ToString();
		}

		public bool IsGameWon()
		{
			return CompareGrids(current, solved);
		}

		// Checks if the given number has been used up.
		// ie if the board has all nine of the given number placed.
		public bool IsNumFull(int n)
		{
			int count = 0;

			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					if(current[i, j] == n && CheckNum(i, j, n)) { count++; }
				}
			}

			return count == 9;
		}
	}
}
