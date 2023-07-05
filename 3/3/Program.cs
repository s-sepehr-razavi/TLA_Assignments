using System;
using System.Collections.Generic;
using System.Linq;
namespace _3
{    
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Dictionary<string, string>>  map = new Dictionary<string, Dictionary<string, string>>();
            string[] keys = Console.ReadLine().Trim('{', '}').Split(',');            
            string[] alphabet = Console.ReadLine().Trim('{', '}').Split(',');
            string[] final_states = Console.ReadLine().Trim('{', '}').Split(',');
            Dictionary<string, string> parents = new Dictionary<string, string>();
            
            set_things_up(map, keys, alphabet, final_states, parents);
            Console.WriteLine(minimalize(map, parents, alphabet));


        }

        static public void find_reachables(Dictionary<string, Dictionary<string, string>> map, string[] keys, HashSet<string> visited, string current_state)
        {
            if (visited.Contains(current_state))
            {
                return;
            }
            visited.Add(current_state);

            Dictionary<string, string> trans = map[current_state];
            foreach (var state in trans.Values)
            {
                find_reachables(map, keys, visited, state);
            }
        } 
        static public void set_things_up(Dictionary<string, Dictionary<string, string>> map, string[] keys, string[] alphabet, string[] final_states, Dictionary<string, string> parents)
        {
            
            foreach (string key in keys)
            {
                //Console.WriteLine(key);
                map.Add(key, new Dictionary<string, string>());
            }

            string[] transition;
            int n = int.Parse(Console.ReadLine());
            for (int i = 0; i < n; i++)
            {
                transition = Console.ReadLine().Split(',');
                map[transition[0]].Add(transition[1], transition[2]);
            }

            HashSet<string> reachable = new HashSet<string>();
            find_reachables(map, keys, reachable, keys[0]);
            HashSet<string> unreachable = new HashSet<string>();
            foreach (string key in keys)
            {
                unreachable.Add(key);
            }
            unreachable.ExceptWith(reachable);
            foreach (var item in unreachable)
            {
                map.Remove(item);
            }

            string not_final = "";            

            HashSet<string> f_state_map = new HashSet<string>();
            foreach (var item in final_states)
            {
                f_state_map.Add(item);
            }

            bool first_time = true;
            foreach (var item in keys)
            {
                if (!f_state_map.Contains(item))
                {
                    if (first_time)
                    {
                        not_final = item;
                        first_time = false;
                    }

                    parents.Add(item, not_final);
                    
                }
                else
                {
                    parents.Add(item, final_states[0]);
                }
            }

        }

        static public int minimalize(Dictionary<string, Dictionary<string, string>> map, Dictionary<string, string> parents, string[] alphabet)
        {
            //Dictionary<string, bool> is_it_taken = new Dictionary<string, bool>(); // meaningless (we are iterating through all values there's no differnce for if they are taken or not and it is not even well-defined)
            //foreach (var item in map.Keys)
            //{
            //    is_it_taken.Add(item, false);
            //}

            string[] keys = map.Keys.ToArray();

            

            int counter = 0;
            
            while (true)
            {
                //return 3;
                //foreach (var item in is_it_taken.Keys)
                //{
                //    is_it_taken[item] = false;
                //}
                Dictionary<string, string> new_parents = new Dictionary<string, string>();
                
                foreach (var key in keys)
                {
                    new_parents.Add(key, key);
                }
                
                for (int i = 0; i < keys.Length; i++)
                {
                    string key_i = keys[i];
                    //if (is_it_taken[key_i])
                    //{
                    //    continue;
                    //}
                    Dictionary<string, string> state_i = map[key_i];
                    
                    for (int j = i + 1; j < keys.Length; j++)
                    {
                        string key_j = keys[j];
                        if (parents[key_i] != parents[key_j])
                        {
                            continue;
                        }
                        Dictionary<string, string> state_j = map[key_j];

                        bool same_set = true;
                        
                        foreach (var letter in alphabet)
                        {
                            if (state_i[letter] != state_j[letter])
                            {
                                if (parents[state_i[letter]] != parents[state_j[letter]])
                                {
                                    same_set = false;
                                    break;
                                }
                            }
                            
                        }

                        //if (!new_parents.ContainsKey(key_i))
                        //    new_parents.Add(key_i, key_i);

                        //if (same_set)
                        //{

                        //    try
                        //    {
                        //        new_parents.Add(key_j, new_parents[key_i]);
                        //    }
                        //    catch (Exception)
                        //    {

                        //        new_parents[key_j] = new_parents[key_i];
                        //    }
                        //}

                        //else
                        //{
                        //    try
                        //    {
                        //        new_parents.Add(key_j, key_j);
                        //    }
                        //    catch (Exception)
                        //    {

                        //        new_parents[key_j] = new_parents[key_j];
                        //    }

                        //}

                        if (same_set)
                        {
                            new_parents[key_j] = new_parents[key_i];
                        }

                    }
                }
                
                //if (new_parents.Count == parents.Count) return new_parents.Count; // bug: size should not change
                //else parents = new_parents;

                HashSet<string> np = new HashSet<string>();
                HashSet<string> p = new HashSet<string>();
                foreach (var val in parents.Values)
                {
                    p.Add(val);
                }

                foreach (var val in new_parents.Values)
                {
                    np.Add(val);
                }

                //if(counter > -1)
                //{
                //    return -1;
                //}
                //counter++;
                if (np.Count == p.Count)
                {
                    return np.Count;
                }
                parents = new_parents;
                
            }
            return 0;
        }

    }

    
}