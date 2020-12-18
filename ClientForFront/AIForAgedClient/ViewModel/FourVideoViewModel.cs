using AIForAgedClient.Helper;

namespace AIForAgedClient.ViewModel
{
    /// <summary>
    /// 用于显示远端视频流
    /// 需要设置视频流地址Url1，Url2,Url3,Url4，序号代表显示框的位置。
    /// </summary>
    public class FourVideoViewModel : BaseFourVideoVM
    {
        private VideoPlayHelper videoPlayHelper1;
        private VideoPlayHelper videoPlayHelper2;
        private VideoPlayHelper videoPlayHelper3;
        private VideoPlayHelper videoPlayHelper4;

        public override void Start()
        {
            if (!string.IsNullOrEmpty(Url1))
            {
                videoPlayHelper1 = new VideoPlayHelper(Url1, (x) => { Image1 = x; });
            }

            if (!string.IsNullOrEmpty(Url2))
            {
                videoPlayHelper2 = new VideoPlayHelper(Url2, (x) => { Image2 = x; });
            }

            if (!string.IsNullOrEmpty(Url3))
            {
                videoPlayHelper3 = new VideoPlayHelper(Url3, (x) => { Image3 = x; });
            }

            if (!string.IsNullOrEmpty(Url4))
            {
                videoPlayHelper4 = new VideoPlayHelper(Url4, (x) => { Image4 = x; });
            }
            videoPlayHelper1?.Start();
            videoPlayHelper2?.Start();
            videoPlayHelper3?.Start();
            videoPlayHelper4?.Start();
        }

        public override void Stop()
        {
            videoPlayHelper1?.Stop();
            videoPlayHelper2?.Stop();
            videoPlayHelper3?.Stop();
            videoPlayHelper4?.Stop();
        }

    }
}
