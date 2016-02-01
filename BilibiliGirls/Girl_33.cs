using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace BilibiliGirls
{
    class Girl_33 : IGirl
    {
        /// <summary>
        /// 移动时的样子
        /// </summary>
        private Image _movingImage = null;
        /// <summary>
        /// 移动时的样子
        /// </summary>
        public Image MovingImage
        {
            get
            {
                return _movingImage;
            }
        }
        /// <summary>
        /// 说话时的样子
        /// </summary>
        private Image _sayingImage = null;
        /// <summary>
        /// 说话时的样子
        /// </summary>
        public Image SayingImage
        {
            get
            {
                return _sayingImage;
            }
        }
        /// <summary>
        /// 被拖拽的呼喊
        /// </summary>
        private KeyValuePair<string, string> dragingSoundKvp = new KeyValuePair<string, string>();
        /// <summary>
        /// 站着时的样子
        /// </summary>
        private List<Image> _standingImageList = new List<Image>();
        /// <summary>
        /// 站着时的样子
        /// </summary>
        public List<Image> StandingImageList
        {
            get
            {
                return _standingImageList;
            }
        }

        private List<KeyValuePair<string, string>> _wordSoundKvpList = new List<KeyValuePair<string, string>>();
        /// <summary>
        /// 文字语音字典(文字-语音文件路径)
        /// </summary>
        public List<KeyValuePair<string, string>> WordSoundKvpList
        {
            get
            {
                return _wordSoundKvpList;
            }
        }

        public Girl_33(ImageList imgList, XmlNode xmlNode)
        {
            #region 初始化图像
            for (int i = 0; i < 5; i++)
            {
                Image img = imgList.Images["33-w" + i.ToString()];
                if (img != null)
                {
                    _standingImageList.Add(img);
                }
            }
            _sayingImage = imgList.Images["33-say"];
            _movingImage = imgList.Images["33-drag"];
            #endregion
            #region 初始化声音
            foreach (XmlNode xn in xmlNode.ChildNodes)
            {
                if (xn.InnerText.ToLower().Contains("drag"))
                {
                    dragingSoundKvp = new KeyValuePair<string, string>(xn.Attributes["word"].Value, Path.Combine(Application.StartupPath, "Sounds", xn.InnerText));
                }
                else
                {
                    _wordSoundKvpList.Add(new KeyValuePair<string, string>(xn.Attributes["word"].Value, Path.Combine(Application.StartupPath, "Sounds", xn.InnerText)));
                }
            }
            #endregion
        }

        /// <summary>
        /// 说话
        /// </summary>
        /// <returns>说话的文字</returns>
        public string Say()
        {
            Random random = new Random();

            KeyValuePair<string, string> kvp = _wordSoundKvpList[random.Next(_wordSoundKvpList.Count)];

            //启动一个新的线程进行声音播放
            Thread sayThread = new Thread(() =>
            {
                Thread.Sleep(100);
                //检查声音文件的存在性
                if (!string.IsNullOrEmpty(kvp.Value) && File.Exists(kvp.Value))
                {
                    System.Media.SoundPlayer sp = new System.Media.SoundPlayer(kvp.Value);
                    sp.Play();
                }
            });
            sayThread.Start();
            return kvp.Key;
        }
        /// <summary>
        /// 被拖拽时呼喊
        /// </summary>
        /// <returns>说话的文字</returns>
        public string DragingSay()
        {
            //启动一个新的线程进行声音播放
            Thread sayThread = new Thread(() =>
            {
                Thread.Sleep(100);
                //检查声音文件的存在性
                if (!string.IsNullOrEmpty(dragingSoundKvp.Value) && File.Exists(dragingSoundKvp.Value))
                {
                    System.Media.SoundPlayer sp = new System.Media.SoundPlayer(dragingSoundKvp.Value);
                    sp.Play();
                }
            });
            sayThread.Start();
            return dragingSoundKvp.Key;
        }
    }
}
