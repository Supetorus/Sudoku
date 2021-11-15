using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
	class Board
	{
		const int size = 9;

		int difficulty = 1;

		int[,] solved = new int[size, size];
		int[,] unsolved = new int[size, size];
		int[,] current = new int[size, size];

		int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

		Random r = new Random();

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
			for (int i = 0; i < difficulty * 21; ++i)
			{
				bool flag = false;

				while (!flag)
				{
					int x = r.Next(0, 9);
					int y = r.Next(0, 9);

					if (unsolved[x, y] != 0)
					{
						unsolved[x, y] = 0;
						flag = true;
					}
				}
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

		bool CheckSafety(int x, int y, int i, int[,] grid)
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

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			for (int y = 0; y < size; ++y)
			{
				for (int x = 0; x < size; ++x)
				{
					sb.Append("   ");
					sb.Append(current[x, y] == 0 ? "  " : current[x, y]);
					sb.Append("   ");
					sb.Append(x == 8 ? "" : "|");
				}

				sb.AppendLine(y == 8 ? "" : "\n------------------------------------------------------");
			}

			return sb.ToString();
		}
	}
}
