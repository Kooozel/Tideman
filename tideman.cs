using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS50
{
    struct Pair
    {
        public int winner;
        public int loser;
    }

    class tideman
    {
        // Max number of candidates
        const int MAX = 9;

        // preferences[i][j] is number of voters who prefer i over j
        int[,] preferences = new int[MAX, MAX];

        // locked[i][j] means i is locked in over j
        bool[,] locked = new bool[MAX, MAX];

        // Each pair has a winner, loser


        // Array of candidates
        string[] candidates = new string[MAX];
        Pair[] pairs = new Pair[MAX * (MAX - 1) / 2];

        int pair_count;
        int candidate_count;


        public void Mainx(string[] args)
        {


            //main(int argc, string argv[])

            // Check for invalid usage
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: tideman [candidate ...]\n");

                //return 1;

            }

            // Populate array of candidates
            candidate_count = args.Length;
            if (candidate_count > MAX)
            {
                Console.WriteLine($"Maximum number of candidates is {MAX}\n");
                //return 2;
            }
            for (int i = 0; i < candidate_count; i++)
            {
                candidates[i] = args[i];
            }

            // Clear graph of locked in pairs
            for (int i = 0; i < candidate_count; i++)
            {
                for (int j = 0; j < candidate_count; j++)
                {
                    locked[i, j] = false;
                }
            }

            pair_count = 0;
            Console.WriteLine("Number of voters: ");
            int voter_count = int.Parse(Console.ReadLine());

            // Query for votes
            for (int i = 0; i < voter_count; i++)
            {
                // ranks[i] is voter's ith preference
                int[] ranks = new int[candidate_count];

                // Query for each rank
                for (int j = 0; j < candidate_count; j++)
                {
                    Console.WriteLine($"Rank {j + 1}: ");
                    string name = Console.ReadLine();

                    if (!vote(j, name, ranks))
                    {
                        Console.WriteLine("Invalid vote.\n");
                        //return 3;
                    }
                }

                record_preferences(ranks);

                Console.WriteLine("\n");
            }

            add_pairs();
            sort_pairs();
            lock_pairs();
            print_winner();

        }

        // Update ranks given a new vote
        bool vote(int rank, string name, int[] ranks)
        {
            for (int i = 0; i < candidate_count; i++)
            {
                if (name == candidates[i])//if (strcmp(name, candidates[i]))
                {
                    ranks[rank] = i;
                    return true;
                }
            }
            return false;

        }

        // Update preferences given one voter's ranks
        void record_preferences(int[] ranks)
        {

            for (int i = 0; i < candidate_count - 1; i++)
            {
                for (int j = candidate_count - 1; j > i; j--)
                {
                    preferences[ranks[i], ranks[j]]++;
                }
            }
            return;
        }

        // Record pairs of candidates where one is preferred over the other
        void add_pairs()
        {
            int count = 0;

            for (int i = 0; i < candidate_count - 1; i++)
            {
                for (int j = candidate_count; j > i; j--)
                {
                    if (preferences[i, j] > preferences[j, i])
                    {
                        pairs[count].winner = i;
                        pairs[count].loser = j;
                        count++;
                    }
                    else if (preferences[i, j] < preferences[j, i])
                    {
                        pairs[count].winner = j;
                        pairs[count].loser = i;
                        count++;
                    }
                }
            }
            pair_count = count;
            return;
        }

        // Sort pairs in decreasing order by strength of victory
        void sort_pairs()
        {
            int index = 0;
            int[] win = new int[pair_count];
            int max = 0;
            for (int i = 0; i < pair_count; i++)
            {
                win[i] = preferences[pairs[i].winner, pairs[i].loser] - preferences[pairs[i].loser, pairs[i].winner];
            }

            for (int j = 0; j < pair_count - 1; j++)
            {
                max = win[j];

                for (int i = j; i < pair_count; i++)
                {
                    if (win[i] >= max)
                    {
                        max = win[i];
                        index = i;
                    }
                }
                Pair[] tmp = new Pair[1];
                tmp[0] = pairs[index];
                pairs[index] = pairs[j];
                pairs[j] = tmp[0];
            }
            return;
        }

        // Lock pairs into the candidate graph in order, without creating cycles
        void lock_pairs()
        {

            for (int i = 0; i < pair_count; i++)
            {
                if (circle(pairs[i].winner, pairs[i].loser) == false)
                {
                    locked[pairs[i].winner, pairs[i].loser] = true;
                }
            }

            return;
        }

        bool circle(int start, int loser)
        {
            if (loser == start)
            {
                return true;
            }
            for (int i = 0; i < candidate_count; i++)
            {
                if (locked[loser, i] == true)
                {
                    if (circle(start, i) == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Print the winner of the election
        void print_winner()
        {

            //int winner;
            int rank;

            for (int i = 0; i < candidate_count; i++)
            {
                rank = 0;
                for (int k = 0; k < candidate_count; k++)
                {
                    if (locked[k, i] == false)
                    {
                        rank++;
                    }
                }

                // Prints all the names that are the source of the graph
                if (rank == candidate_count)
                {
                    Console.WriteLine($"{candidates[i]}");
                }
            }
        }


    }
}
