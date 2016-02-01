using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace BilibiliGirls
{
    internal interface IGirl
    {
        /// <summary>
        /// 文字语音键值对
        /// </summary>
        List<KeyValuePair<string, string>> WordSoundKvpList { get; }
        /// <summary>
        /// 说一句话
        /// </summary>
        string Say();
        /// <summary>
        /// 拖拽时的呼喊
        /// </summary>
        string DragingSay();
        /// <summary>
        /// 说话时的样子
        /// </summary>
        Image SayingImage { get; }
        /// <summary>
        /// 移动时的样子
        /// </summary>
        Image MovingImage { get; }
        /// <summary>
        /// 站着时的样子
        /// </summary>
        List<Image> StandingImageList { get; }
    }
}
