using System.Text;

namespace ApiProductOrders.Views
{
    public class ViewNotes
    {
        public int totalNotesCount { get; set; }
        public int uniqueNotesOrderIdCount { get; set; }
        public int deletedOrdersCount { get; set; }
        public int remainingOrders { get; set; }
        public int updatedOrders { get; set; }
        public int notesFailureCount { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Total de registros importados {this.totalNotesCount}");
            sb.AppendLine($"OrderId únicas {this.uniqueNotesOrderIdCount}");
            sb.AppendLine($"Operações deletadas {this.deletedOrdersCount}");
            sb.AppendLine($"Operações restantes {this.remainingOrders}");
            sb.AppendLine($"Ordens atualizadas {this.updatedOrders}");
            sb.AppendLine($"Falha de Apontamento {this.notesFailureCount}");
            return sb.ToString();
        }
    }
}
