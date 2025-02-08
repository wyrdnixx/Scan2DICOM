using NTwain.Data;
using NTwain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using System.Drawing.Imaging;
//using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Drawing.Printing;

namespace Scan2Dicom
{

  
    
        public  class TwainScanner

    {

        public event EventHandler LogEvent;
        public event ImgEventHandler ImageEvent;


        ImageCodecInfo _tiffCodecInfo;
        TwainSession _twain;
        bool _stopScan;
        bool _loadingCaps;


        #region setup & cleanup

        public TwainScanner()
        {


            //InitializeComponent();
            if (NTwain.PlatformInfo.Current.IsApp64Bit)
            {
               // Text = Text + " (64bit)";
            }
            else
            {
                //Text = Text + " (32bit)";
            }
            foreach (var enc in ImageCodecInfo.GetImageEncoders())
            {
                //if (enc.MimeType == "image/tiff") { _tiffCodecInfo = enc; break; }
                if (enc.MimeType == "image/tiff") { _tiffCodecInfo = enc; break; }
            }
            
            // own
           //SetupTwain();

        }



        public  void SetupTwain()
        {
            LogEvent("SetupTwain...");

            var appId = TWIdentity.CreateFromAssembly(DataGroups.Image, Assembly.GetEntryAssembly());
            _twain = new TwainSession(appId);
            _twain.StateChanged += (s, e) =>
            {
                //PlatformInfo.Current.Log.Info("State changed to " + _twain.State + " on thread " + Thread.CurrentThread.ManagedThreadId);
                LogEvent("Scanner state changed to " + _twain.State + " on thread " + Thread.CurrentThread.ManagedThreadId);
            };
            _twain.TransferError += (s, e) =>
            {
                LogEvent("Got xfer error on thread " + Thread.CurrentThread.ManagedThreadId);
            };
            _twain.DataTransferred += (s, e) =>
            {
                LogEvent("Transferred data event on thread " + Thread.CurrentThread.ManagedThreadId);

                // example on getting ext image info
                var infos = e.GetExtImageInfo(ExtendedImageInfo.Camera).Where(it => it.ReturnCode == ReturnCode.Success);
                foreach (var it in infos)
                {
                    var values = it.ReadValues();
                    LogEvent(string.Format("{0} = {1}", it.InfoID, values.FirstOrDefault()));
                    break;
                }

                // handle image data
                Image img = null;
                if (e.NativeData != IntPtr.Zero)
                {
                    var stream = e.GetNativeImageStream();
                    if (stream != null)
                    {
                        img = Image.FromStream(stream);
                    }
                }
                else if (!string.IsNullOrEmpty(e.FileDataPath))
                {
                    img = new Bitmap(e.FileDataPath);
                }
                if (img != null)
                {
                    /* this.BeginInvoke(new Action(() =>
                     {
                         if (pictureBox1.Image != null)
                         {
                             pictureBox1.Image.Dispose();
                             pictureBox1.Image = null;
                         }
                         pictureBox1.Image = img;
                     }));*/
                    //LogEvent("img success");
                    LogEvent("img success: "+ img.RawFormat);
                    ImageEvent(img);
                }
            };
            _twain.SourceDisabled += (s, e) =>
            {
                PlatformInfo.Current.Log.Info("Source disabled event on thread " + Thread.CurrentThread.ManagedThreadId);
                /*this.BeginInvoke(new Action(() =>
                {
                    btnStopScan.Enabled = false;
                    btnStartCapture.Enabled = true;
                    panelOptions.Enabled = true;
                    LoadSourceCaps();
                }));*/
            };
            _twain.TransferReady += (s, e) =>
            {
                PlatformInfo.Current.Log.Info("Transferr ready event on thread " + Thread.CurrentThread.ManagedThreadId);
                e.CancelAll = _stopScan;
            };

            // either set sync context and don't worry about threads during events,
            // or don't and use control.invoke during the events yourself
            PlatformInfo.Current.Log.Info("Setup thread = " + Thread.CurrentThread.ManagedThreadId);
            _twain.SynchronizationContext = SynchronizationContext.Current;
            if (_twain.State < 3)
            {
                // use this for internal msg loop
                _twain.Open();
                // use this to hook into current app loop
                //_twain.Open(new WindowsFormsMessageLoopHook(this.Handle));
            }
        }

        private void CleanupTwain()
        {
            if (_twain.State == 4)
            {
                _twain.CurrentSource.Close();
            }
            if (_twain.State == 3)
            {
                _twain.Close();
            }

            if (_twain.State > 2)
            {
                // normal close down didn't work, do hard kill
                _twain.ForceStepDown(2);
            }
        }

        #endregion

        #region toolbar

        private void btnSources_DropDownOpening(object sender, EventArgs e)
        {
            /* if (btnSources.DropDownItems.Count == 2)
             {
                 ReloadSourceList();
             }*/
            ReloadSourceList();
        }

        private void reloadSourcesListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadSourceList();
        }

        public List<string> ReloadSourceList()
        {
            List<string> Scannerlist = new List<string>();
            /*  if (_twain.State >= 3)
              {
                  while (btnSources.DropDownItems.IndexOf(sepSourceList) > 0)
                  {
                      var first = btnSources.DropDownItems[0];
                      first.Click -= SourceMenuItem_Click;
                      btnSources.DropDownItems.Remove(first);
                  }
                  foreach (var src in _twain)
                  {
                      var srcBtn = new ToolStripMenuItem(src.Name);
                      srcBtn.Tag = src;
                      srcBtn.Click += SourceMenuItem_Click;
                      srcBtn.Checked = _twain.CurrentSource != null && _twain.CurrentSource.Name == src.Name;
                      btnSources.DropDownItems.Insert(0, srcBtn);
                  }
              }*/
            if (_twain.State >= 3)
            {
                foreach (var src in _twain)
                {
                    LogEvent("Twain Scanner found: " + src.Name);
                    Scannerlist.Add(src.Name);
                }
            }
            return Scannerlist;
        }

        public void selectTwainScanner()
        {
            if (_twain.CurrentSource != null)
                _twain.CurrentSource.Close();
            DataSource src = _twain.ShowSourceSelector();
            if (src.Open() == ReturnCode.Success)
            {
                LogEvent("Scanner geladen");
               // LoadSourceCaps();
            } else
            {
                LogEvent("Scanner konnte nicht geladen werden.");
            }
            
        }

        void SourceMenuItem_Click(object sender, EventArgs e)
        {
            // do nothing if source is enabled
            if (_twain.State > 4) { return; }

            if (_twain.State == 4) { _twain.CurrentSource.Close(); }
/*
            foreach (var btn in btnSources.DropDownItems)
            {
                var srcBtn = btn as ToolStripMenuItem;
                if (srcBtn != null) { srcBtn.Checked = false; }
            }

            var curBtn = (sender as ToolStripMenuItem);
            var src = curBtn.Tag as DataSource;
            if (src.Open() == ReturnCode.Success)
            {
                curBtn.Checked = true;
                btnStartCapture.Enabled = true;
                LoadSourceCaps();
            }*/
        }


        public void StartScan(IntPtr handle)
        {
            if (_twain.State == 4)
            {
                //_twain.CurrentSource.CapXferCount.Set(4);

                _stopScan = false;

                if (_twain.CurrentSource.Capabilities.CapUIControllable.IsSupported)//.SupportedCaps.Contains(CapabilityId.CapUIControllable))
                {
                    // hide scanner ui if possible
                    if (_twain.CurrentSource.Enable(SourceEnableMode.NoUI, false, handle) == ReturnCode.Success)
                    {
                        /*btnStopScan.Enabled = true;
                        btnStartCapture.Enabled = false;
                        panelOptions.Enabled = false;*/
                    }
                }
                else
                {
                    if (_twain.CurrentSource.Enable(SourceEnableMode.ShowUI, true, handle) == ReturnCode.Success)
                    {
                        /*                        btnStopScan.Enabled = true;
                                                btnStartCapture.Enabled = false;
                                                panelOptions.Enabled = false;
                        */
                    }
                }
            }
        }



            /*  private void btnStartCapture_Click(object sender, EventArgs e)
              {
                  if (_twain.State == 4)
                  {
                      //_twain.CurrentSource.CapXferCount.Set(4);

                      _stopScan = false;

                      if (_twain.CurrentSource.Capabilities.CapUIControllable.IsSupported)//.SupportedCaps.Contains(CapabilityId.CapUIControllable))
                      {
                          // hide scanner ui if possible
                          if (_twain.CurrentSource.Enable(SourceEnableMode.NoUI, false, this.Handle) == ReturnCode.Success)
                          {
                              btnStopScan.Enabled = true;
                              btnStartCapture.Enabled = false;
                              panelOptions.Enabled = false;
                          }
                      }
                      else
                      {
                          if (_twain.CurrentSource.Enable(SourceEnableMode.ShowUI, true, this.Handle) == ReturnCode.Success)
                          {
                              btnStopScan.Enabled = true;
                              btnStartCapture.Enabled = false;
                              panelOptions.Enabled = false;
                          }
                      }
                  }
              }*/

            /*        private void btnStopScan_Click(object sender, EventArgs e)
                    {
                        _stopScan = true;
                    }

                    private void btnSaveImage_Click(object sender, EventArgs e)
                    {
                        var img = pictureBox1.Image;

                        if (img != null)
                        {
                            switch (img.PixelFormat)
                            {
                                case PixelFormat.Format1bppIndexed:
                                    saveFileDialog1.Filter = "tiff files|*.tif";
                                    break;
                                default:
                                    saveFileDialog1.Filter = "png files|*.png";
                                    break;
                            }

                            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                if (saveFileDialog1.FileName.EndsWith(".tif", StringComparison.OrdinalIgnoreCase))
                                {
                                    EncoderParameters tiffParam = new EncoderParameters(1);

                                    tiffParam.Param[0] = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionCCITT4);

                                    pictureBox1.Image.Save(saveFileDialog1.FileName, _tiffCodecInfo, tiffParam);
                                }
                                else
                                {
                                    pictureBox1.Image.Save(saveFileDialog1.FileName, ImageFormat.Png);
                                }
                            }
                        }
                    }*/

            #endregion

            #region cap control

            /*
                    private void LoadSourceCaps()
                    {
                        var src = _twain.CurrentSource;
                        _loadingCaps = true;

                        //var test = src.SupportedCaps;

                        if (groupDepth.Enabled = src.Capabilities.ICapPixelType.IsSupported)
                        {
                            LoadDepth(src.Capabilities.ICapPixelType);
                        }
                        if (groupDPI.Enabled = src.Capabilities.ICapXResolution.IsSupported && src.Capabilities.ICapYResolution.IsSupported)
                        {
                            LoadDPI(src.Capabilities.ICapXResolution);
                        }
                        // TODO: find out if this is how duplex works or also needs the other option
                        if (groupDuplex.Enabled = src.Capabilities.CapDuplexEnabled.IsSupported)
                        {
                            LoadDuplex(src.Capabilities.CapDuplexEnabled);
                        }
                        if (groupSize.Enabled = src.Capabilities.ICapSupportedSizes.IsSupported)
                        {
                            LoadPaperSize(src.Capabilities.ICapSupportedSizes);
                        }
                        btnAllSettings.Enabled = src.Capabilities.CapEnableDSUIOnly.IsSupported;
                        _loadingCaps = false;
                    }

                    private void LoadPaperSize(ICapWrapper<SupportedSize> cap)
                    {
                        var list = cap.GetValues().ToList();
                        comboSize.DataSource = list;
                        var cur = cap.GetCurrent();
                        if (list.Contains(cur))
                        {
                            comboSize.SelectedItem = cur;
                        }
                        var labelTest = cap.GetLabel();
                        if (!string.IsNullOrEmpty(labelTest))
                        {
                            groupSize.Text = labelTest;
                        }
                    }


                    private void LoadDuplex(ICapWrapper<BoolType> cap)
                    {
                        ckDuplex.Checked = cap.GetCurrent() == BoolType.True;
                    }


                    private void LoadDPI(ICapWrapper<TWFix32> cap)
                    {
                        // only allow dpi of certain values for those source that lists everything
                        var list = cap.GetValues().Where(dpi => (dpi % 50) == 0).ToList();
                        comboDPI.DataSource = list;
                        var cur = cap.GetCurrent();
                        if (list.Contains(cur))
                        {
                            comboDPI.SelectedItem = cur;
                        }
                    }

                    private void LoadDepth(ICapWrapper<PixelType> cap)
                    {
                        var list = cap.GetValues().ToList();
                        comboDepth.DataSource = list;
                        var cur = cap.GetCurrent();
                        if (list.Contains(cur))
                        {
                            comboDepth.SelectedItem = cur;
                        }
                        var labelTest = cap.GetLabel();
                        if (!string.IsNullOrEmpty(labelTest))
                        {
                            groupDepth.Text = labelTest;
                        }
                    }

                    private void comboSize_SelectedIndexChanged(object sender, EventArgs e)
                    {
                        if (!_loadingCaps && _twain.State == 4)
                        {
                            var sel = (SupportedSize)comboSize.SelectedItem;
                            _twain.CurrentSource.Capabilities.ICapSupportedSizes.SetValue(sel);
                        }
                    }

                    private void comboDepth_SelectedIndexChanged(object sender, EventArgs e)
                    {
                        if (!_loadingCaps && _twain.State == 4)
                        {
                            var sel = (PixelType)comboDepth.SelectedItem;
                            _twain.CurrentSource.Capabilities.ICapPixelType.SetValue(sel);
                        }
                    }

                    private void comboDPI_SelectedIndexChanged(object sender, EventArgs e)
                    {
                        if (!_loadingCaps && _twain.State == 4)
                        {
                            var sel = (TWFix32)comboDPI.SelectedItem;
                            _twain.CurrentSource.Capabilities.ICapXResolution.SetValue(sel);
                            _twain.CurrentSource.Capabilities.ICapYResolution.SetValue(sel);
                        }
                    }

                    private void ckDuplex_CheckedChanged(object sender, EventArgs e)
                    {
                        if (!_loadingCaps && _twain.State == 4)
                        {
                            _twain.CurrentSource.Capabilities.CapDuplexEnabled.SetValue(ckDuplex.Checked ? BoolType.True : BoolType.False);
                        }
                    }

                    private void btnAllSettings_Click(object sender, EventArgs e)
                    {
                        _twain.CurrentSource.Enable(SourceEnableMode.ShowUIOnly, true, this.Handle);
                    }*/

            #endregion




        }

}
