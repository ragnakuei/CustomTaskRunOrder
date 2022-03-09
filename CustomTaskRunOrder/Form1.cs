using System.Collections.Concurrent;
using KueiExtensions;

namespace CustomTaskRunOrder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            InitialActionList();

            InitialGroupBoxFlow();
        }

        #region ListBoxActions

        private Dictionary<string, Func<ContextDto, Task>> _actions;

        private void InitialActionList()
        {
            _actions = new Dictionary<string, Func<ContextDto, Task>>
                       {
                           ["Action1"] = (dto) => Action1(dto),
                           ["Action2"] = (dto) => Action2(dto),
                           ["Action3"] = (dto) => Action3(dto),
                           ["Delay"]   = (dto) => Delay(dto),
                       };

            lbxActions.Items.AddRange(_actions.Keys.ToArray());
        }

        #region Actions

        private async Task Delay(ContextDto dto)
        {
            dto.StringResult.Add($"Delay {dto.DelayTime}");

            await Task.Delay(dto.DelayTime);
        }

        private async Task Action1(ContextDto dto)
        {
            dto.StringResult.Add("Action1");

            await Task.CompletedTask;
        }

        private async Task Action2(ContextDto dto)
        {
            dto.StringResult.Add("Action2");

            await Task.CompletedTask;
        }

        private async Task Action3(ContextDto dto)
        {
            dto.StringResult.Add("Action3");

            await Task.CompletedTask;
        }

        #endregion

        #endregion

        #region GroupBoxFlow

        /// <summary>
        /// 在 GroupBox Flow ，先往下，再往右
        /// </summary>
        private List<List<Control>> _flowControls = new();

        private readonly int _groupBoxFlowButtonSizeWidth  = 80;
        private readonly int _groupBoxFlowButtonSizeHeight = 60;

        private void InitialGroupBoxFlow()
        {
            _flowControls = new List<List<Control>>
                            {
                                new List<Control>
                                {
                                    GeneratGroupBoxFlowButton("Add")
                                }
                            };

            ReRenderGroupBoxFlowControls();
        }


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
            if (lbxActions.SelectedItem is not string selectedItem)
            {
                return;
            }

            var targetButton = sender as Button;
            targetButton.Text    = selectedItem;
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

        #endregion

        private async void buttonRun_Click(object sender, EventArgs e)
        {
            SynchronizationContext.Current.Post(_ =>
                                                {
                                                    rtbxResult.Text  = string.Empty;
                                                    btnRun.Text      = "Running";
                                                    btnRun.Enabled   = false;
                                                    btnReset.Enabled = false;
                                                },
                                                null);

            var contextDto = new ContextDto
                             {
                                 // delay 先固定給定 2 秒
                                 DelayTime    = 2000,
                                 StringResult = new ConcurrentBag<string>()
                             };

            foreach (var flowGroup in _flowControls)
            {
                Task[] parallelTasks = flowGroup.Select(c => _actions.GetValueOrDefault(c.Text))
                                                .Where(t => t != null)
                                                .Select(t => t.Invoke(contextDto))
                                                .ToArray();

                await Task.WhenAll(parallelTasks);

                var output = contextDto.StringResult.Join("\n");

                SynchronizationContext.Current.Post(_ =>
                                                    {
                                                        rtbxResult.Text = output;
                                                    },
                                                    null);
            }

            SynchronizationContext.Current.Post(_ =>
                                                {
                                                    btnRun.Text      = "Run";
                                                    btnRun.Enabled   = true;
                                                    btnReset.Enabled = true;
                                                },
                                                null);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            InitialGroupBoxFlow();
            rtbxResult.Text = string.Empty;
        }
    }
}
