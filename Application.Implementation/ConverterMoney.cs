using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Implementation
{
    public class Cgraf
    {
        private int[,] mAdyacencia;
        private int[] indegree;
        private int nodes;

        public Cgraf(int pnodes)
        {
            nodes = pnodes;
            mAdyacencia = new int[nodes, nodes];
            indegree = new int[nodes];
        }

        public void AddArist(int nodeinitial, int nodeEnd)
        {
            mAdyacencia[nodeinitial, nodeEnd] = 1;
        }

        public int getAdyacencia(int row, int column)
        {
            return mAdyacencia[row, column];
        }

        public void calculateIndegree()
        {
            int n = 0;
            int m = 0;

            for (n = 0; n < nodes; n++)
            {
                for (m = 0; m < nodes; m++)
                {
                    if (mAdyacencia[m, n] == 1)
                    {
                        indegree[n]++;
                    }
                }
            }
        }
        public int calculateIo()
        {
            bool finded = false;
            int n = 0;
            for (n = 0; n < nodes; n++)
            {
                if (indegree[n] == 0)
                {
                    finded = true;
                    break;
                }
            }
            if (finded)
            {
                return n;
            }
            else
            {
                return -1;
            }
        }

        public void decrementalIndigree(int node)
        {
            indegree[node] = -1;
            int n = 0;
            for (n = 0; n < nodes; n++)
            {
                if (mAdyacencia[node, n] == 1)
                {
                    indegree[n]--;
                }
            }


        }
    }

    public class ConverterMoney
    {
        private IEnumerable<Rates> _rates;
        public ConverterMoney(IEnumerable<Rates> rates)
        {
            _rates = rates;
        }

        public List<string> CreateRouteToConvertion(string currencyFrom, string currencyTo)
        {

            var map = new List<string>();

            var listnodes = GetDistinctCurrencies();

            int startnode = 0;
            int endNode = 0;
            int distance = 0;
            int n = 0;
            int m = 0;
            int totalnodes = listnodes.Count();
            string data = "";

            Cgraf mygraf = new Cgraf(totalnodes);

            var counterindex = 0;
            foreach (var item in listnodes)
            {
                var arists = _rates.Where(r => r.From == item).Select(s => s.To);
                foreach (var arist in arists)
                {
                    var indexTo = GetIndexByValue(listnodes, arist);
                    mygraf.AddArist(counterindex, indexTo);
                }
                counterindex++;
            }
            startnode = GetIndexByValue(listnodes, currencyFrom);
            endNode = GetIndexByValue(listnodes, currencyTo);


            //create  table
            //0 - visited
            //1 - distance
            //2 - preview

            int[,] table = new int[totalnodes, 3];
            for (n = 0; n < totalnodes; n++)
            {
                table[n, 0] = 0;
                table[n, 1] = int.MaxValue;
                table[n, 2] = 0;
            }
            table[startnode, 1] = 0;

            //run all nodes

            for (distance = 0; distance < totalnodes; distance++)
            {
                for (n = 0; n < totalnodes; n++)
                {
                    if ((table[n, 0] == 0) && (table[n, 1] == distance))
                    {
                        table[n, 0] = 1;
                        for (m = 0; m < totalnodes; m++)
                        {
                            //verify node its adyacent 
                            if (mygraf.getAdyacencia(n, m) == 1)
                            {
                                if (table[m, 1] == int.MaxValue)
                                {
                                    table[m, 1] = distance + 1;
                                    table[m, 2] = n;

                                }
                            }

                        }

                    }

                }
            }

            //get route 

            List<int> route = new List<int>();
            int node = endNode;
            while (node != startnode)
            {
                route.Add(node);
                node = table[node, 2];
            }
            route.Add(startnode);
            route.Reverse();

            foreach (var item in route)
            {
                map.Add(listnodes[item]);
            }

            return map;

        }
        public int GetIndexByValue(List<string> matriz, string value)
        {
            var index = -1;
            for (int i = 0; i < matriz.Count; i++)
            {
                if (matriz[i] == value)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        public List<string> GetDistinctCurrencies()
        {
            var lis = new List<string>();
            lis.AddRange(_rates.Select(r => r.From).Distinct());
            lis.AddRange(_rates.Select(r => r.To).Distinct());

            lis = lis.OrderBy(s=>s).Distinct().ToList();
            return lis;
        }     


        public List<string> GetMapToConvertionLineal(string currencyFrom, string currencyTo)
        {


            var map = new List<string>();                        

            //no hay tasa para convertir
            if (!_rates.Any(s => s.To == currencyTo))
            {
                throw new Exception("No hay Modo de conversion ");
            }
            // conversion directa
            if (_rates.Any(s => s.From == currencyFrom && s.To == currencyTo))
            {
                map.Add(currencyFrom);
                map.Add(currencyTo);
                return map;
            }

            //generar mapa de conversion complejo 
            map = CreateRouteToConvertion(currencyFrom, currencyTo);        

            return map;
        }
        public decimal ConvertTo(decimal fromvalue, string currencyFrom, string currencyTo)
        {
            decimal value = 0;


            //DirectConvertion
            if (_rates.Any(s => s.From == currencyFrom && s.To == currencyTo))
            {
                var rate = _rates.Where(s => s.From == currencyFrom && s.To == currencyFrom).First().Rate;
                value = fromvalue * rate;
                return value;
            }

            return value;
        }
        public decimal ConvertTo(decimal fromvalue, List<string> convertionMap)
        {
            decimal value = 0;

            for (int i = 1; i < convertionMap.Count; i++)
            {
                var rate = _rates.Where(s => s.From == convertionMap[i-1] && s.To == convertionMap[i]).First().Rate;
                value = fromvalue * rate;
            }
          
            return value;
        }
    }
}
