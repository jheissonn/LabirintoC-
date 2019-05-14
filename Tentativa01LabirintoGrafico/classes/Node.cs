using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tentativa01LabirintoGrafico
{

  public  class Node<T>
    {
        private T item;
        private Node<T> proximo;

        public Node(T item)
        {
            this.item = item;
        }

        public T Item
        {
            get { return this.item; }
            set { this.item = value; }
        }
        public Node<T> Proximo
        {
            get { return this.proximo; }
            set { this.proximo = value; }
        }
    }
}
