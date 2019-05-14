using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tentativa01LabirintoGrafico
{

    class Pilha<T>
    {
        public Node<T> head { get; private set; }
        public Node<T> end { get; private set; }
        public int qtde { get; private set; }
        public Pilha()
        {
            this.end = null;
            this.head = null;
            this.qtde = 0;
        }

        /* Retorna se a lista está vazia ou não*/
        public bool IsEmpty
        {
            get { return this.qtde == 0; }
        }

        /* Remove o último elemento da lista. */
        public void RemoveLast()
        {
            if (head == null)
            {
                return;
            }
            if (head.Proximo == null)
            {
                head = null;
            }
            else
            {
                Node<T> last = head.Proximo;
                Node<T> penultimate = head;

                while (last.Proximo != null)
                {
                    penultimate = last;
                    last = last.Proximo;
                }
                penultimate.Proximo = null;
                end = penultimate;
            }
            qtde--;
        }

        public T peek() {
            return this.end.Item;
        }
        public void InsertLast(T temp)
        {

            //Cria o nó com os valores que serão recebidos
            Node<T> newNode = new Node<T>(temp);
            //Novo nó recebe o valor 
            //Verifica se é o primeiro
            if (head == null)
            {
                head = newNode;
                end = newNode;
            }
            else
            {
                //Adiciona ao próximo da lista o novo valor
                end.Proximo = newNode;
            }
            //Adiciona a calda o novo valor
            end = newNode;
            qtde++;
        }

    }
}
