using System.Collections.Concurrent;
using KueiExtensions;

namespace CustomTaskRunOrder
{
    public partial class Form1 : Form
    {
        private readonly CustomTaskRunService<ContextDto> _customTaskRunService;

        public Form1()
        {
            InitializeComponent();

            _customTaskRunService = new CustomTaskRunService<ContextDto>();

            InitialActionList();

            InitialGroupBoxFlow();
        }

        #region ListBoxActions

        private void InitialActionList()
        {
            _customTaskRunService.Actions = new Dictionary<string, Func<ContextDto, Task>>
                                            {
                                                ["Action1"] = (dto) => Action1(dto),
                                                ["Action2"] = (dto) => Action2(dto),
                                                ["Action3"] = (dto) => Action3(dto),
                                                ["Delay"]   = (dto) => Delay(dto),
                                            };

            lbxActions.Items.AddRange(_customTaskRunService.Actions.Keys.ToArray());
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
        private List<List<Control>> _groupBoxFlowControls = new();

        private readonly int _groupBoxFlowButtonSizeWidth  = 80;
        private readonly int _groupBoxFlowButtonSizeHeight = 60;

        private void InitialGroupBoxFlow()
        {
            _customTaskRunService.TaskGroups = new List<List<string>>
                                               {
                                                   new(),
                                               };

            GenerateGroupBoxFlowControls();
            ReRenderGroupBoxFlowControls();
        }

        private void GenerateGroupBoxFlowControls()
        {
            _groupBoxFlowControls = new List<List<Control>>();

            if (_customTaskRunService.TotalTasksCount == 0)
            {
                _groupBoxFlowControls.Add(new List<Control>
                                          {
                                              GeneratGroupBoxFlowButton("Add"),
                                          });
                return;
            }

            for (int y = 0; y < _customTaskRunService.TaskGroups.Count; y++)
            {
                _groupBoxFlowControls.Add(new List<Control>());

                for (int x = 0; x < _customTaskRunService.TaskGroups[y].Count; x++)
                {
                    _groupBoxFlowControls[y].Add(GeneratGroupBoxFlowButton(_customTaskRunService.TaskGroups[y][x], false));
                }

                _groupBoxFlowControls[y].Add(GeneratGroupBoxFlowButton("Add Parallel Task"));
            }

            _groupBoxFlowControls.Add(new List<Control>
                                      {
                                          GeneratGroupBoxFlowButton("Add Sequence Task"),
                                      });
        }


        private Button GeneratGroupBoxFlowButton(string text, bool enabled = true)
        {
            var button = new Button
                         {
                             Size      = new Size(_groupBoxFlowButtonSizeWidth, _groupBoxFlowButtonSizeHeight),
                             Text      = text,
                             TextAlign = ContentAlignment.MiddleCenter,
                             Enabled   = enabled,
                         };

            button.Click += new EventHandler(GroupBoxFlowButton_Click);
            return button;
        }

        private void ReRenderGroupBoxFlowControls()
        {
            gbxFlow.Controls.Clear();

            var startY = 20;
            var spaceY = _groupBoxFlowButtonSizeHeight + 10;

            for (int y = 0; y < _groupBoxFlowControls.Count; y++)
            {
                var startX = 10;
                var spaceX = _groupBoxFlowButtonSizeWidth + 10;

                for (int x = 0; x < _groupBoxFlowControls[y].Count; x++)
                {
                    var control = _groupBoxFlowControls[y][x];
                    control.Top  = startY + spaceY * y;
                    control.Left = startX + spaceX * x;
                    control.Tag  = $"{y}-{x}";

                    gbxFlow.Controls.Add(_groupBoxFlowControls[y][x]);
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
            var tagString    = targetButton?.Tag.ToString();
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

            if (indexY.Value == 0
             && indexX.Value == 0)
            {
                _customTaskRunService.TaskGroups[indexY.Value].Add(selectedItem);
            }
            else if (indexX.Value == 0)
            {
                _customTaskRunService.TaskGroups.Add(new List<string>
                                                     {
                                                         selectedItem
                                                     });
            }
            else
            {
                _customTaskRunService.TaskGroups[indexY.Value].Add(selectedItem);
            }

            GenerateGroupBoxFlowControls();
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

            await _customTaskRunService.Run(contextDto,
                                            () =>
                                            {
                                                var output = contextDto.StringResult.Join("\n");

                                                SynchronizationContext.Current.Post(_ =>
                                                                                    {
                                                                                        rtbxResult.Text = output;
                                                                                    },
                                                                                    null);
                                            });

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
