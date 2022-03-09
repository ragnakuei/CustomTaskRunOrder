using KueiExtensions;

namespace CustomTaskRunOrder
{
    public partial class Form1 : Form
    {
        private Dictionary<string, Action<ContextDto>> _actions;

        public Form1()
        {
            InitializeComponent();

            InitialActionList();

            InitialGroupBoxFlow();
        }

        private void InitialActionList()
        {
            _actions = new Dictionary<string, Action<ContextDto>>
                       {
                           ["Action1"] = (dto) => Action1(dto),
                           ["Action2"] = (dto) => Action2(dto),
                           ["Action3"] = (dto) => Action3(dto),
                           ["Delay"]   = (dto) => Delay(dto),
                       };
        }

        /// <summary>
        /// 在 GroupBox Flow ，先往下，再往右
        /// </summary>
        private List<List<Control>> _flowControls = new();

        private void InitialGroupBoxFlow()
        {
            _flowControls.Add(new List<Control>
                              {
                                  GeneratGroupBoxFlowButton("Add")
                              });

            ReRenderGroupBoxFlowControls();
        }

        private readonly int _groupBoxFlowButtonSizeWidth  = 120;
        private readonly int _groupBoxFlowButtonSizeHeight = 60;

        private Button GeneratGroupBoxFlowButton(string text)
        {
            var button = new Button
                         {
                             Size      = new Size(_groupBoxFlowButtonSizeWidth, _groupBoxFlowButtonSizeHeight),
                             Text      = text,
                             TextAlign = ContentAlignment.MiddleCenter,
                         };

            button.Click += new EventHandler(GroupBoxFlowButton_Click);
            return button;
        }

        private void ReRenderGroupBoxFlowControls()
        {
            gbxFlow.Controls.Clear();

            var startY = 20;
            var spaceY = _groupBoxFlowButtonSizeHeight + 10;

            for (int y = 0; y < _flowControls.Count; y++)
            {
                var startX = 10;
                var spaceX = _groupBoxFlowButtonSizeWidth + 10;

                for (int x = 0; x < _flowControls[y].Count; x++)
                {
                    var control = _flowControls[y][x];
                    control.Top  = startY + spaceY * y;
                    control.Left = startX + spaceX * x;
                    control.Tag  = $"{y}-{x}";

                    gbxFlow.Controls.Add(_flowControls[y][x]);
                }
            }
        }

        private void GroupBoxFlowButton_Click(object? sender, EventArgs e)
        {
            var targetButton = sender as Button;
            targetButton.Enabled = false;

            var tagString = targetButton?.Tag.ToString();
            if (tagString.IsNullOrWhiteSpace())
            {
                return;
            }

            var indexs = tagString.Split('-');
            var indexY = indexs[0].ToNullableInt();
            var indexX = indexs[1].ToNullableInt();

            if (indexY.HasValue == false
             || indexX.HasValue == false)
            {
                return;
            }

            // 加 右
            var yControls = _flowControls[indexY.Value];
            yControls.Add(GeneratGroupBoxFlowButton("Add Parallel Task"));

            if (indexX == 0)
            {
                // 加 下
                _flowControls.Add(new List<Control>
                                  {
                                      GeneratGroupBoxFlowButton("Add Sequence Task")
                                  });
            }

            ReRenderGroupBoxFlowControls();
        }

        private async Task Delay(ContextDto dto)
        {
            await Task.Delay(dto.DelayTime);
        }

        private async Task Action1(ContextDto dto)
        {
            dto.StringResult = "Action1";

            await Task.CompletedTask;
        }

        private async Task Action2(ContextDto dto)
        {
            dto.StringResult = "Action2";

            await Task.CompletedTask;
        }

        private async Task Action3(ContextDto dto)
        {
            dto.StringResult = "Action3";

            await Task.CompletedTask;
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}
