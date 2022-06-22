namespace ModelApp.AppTools.Views
{
    public partial class MainView : Form
    {
        private readonly CypherView _cypherView;
        private readonly DBICView _dbICView;

        public MainView(CypherView cypherView, DBICView dbICView)
        {
            _cypherView = cypherView;
            _dbICView = dbICView;
            InitializeComponent();
        }

        private void cypherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _cypherView.MdiParent = this;
            _cypherView.Show();
        }

        private void dBICToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _dbICView.MdiParent = this;
            _dbICView.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
