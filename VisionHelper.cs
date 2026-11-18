using HalconDotNet;          // 新增：Halcon .NET 接口
using System;

using System.IO;


namespace WindowsFormsApp1
{
    public static class VisionHelper
    {
        public static string RunVisionProcess()
        {
            HImage img = null;
            try
            {
                // 1. 图片路径：指向真实的 frame_000.png（原来写的 test.jpg 是个目录）
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string imagePath = Path.Combine(baseDir, "test.jpg", "frame_000.png");
                if (!File.Exists(imagePath))
                    return "找不到图片：" + imagePath;

                // 2. 读图 + 拆 RGB 三通道
                img = new HImage(imagePath);
                HObject hr, hg, hb;
                HOperatorSet.Decompose3(img, out hr, out hg, out hb);
                HImage r = new HImage(hr);
                HImage g = new HImage(hg);
                HImage b = new HImage(hb);




                // 3. 红色阈值：R 高、G/B 低
                //    实测围巾 RGB=(169,29,35)，故 R:130~255, G:0~90, B:0~90
                HRegion redR = r.Threshold(130.0, 255.0);
                HRegion redG = g.Threshold(0.0, 90.0);
                HRegion redB = b.Threshold(0.0, 90.0);

                HRegion red = redR.Intersection(redG).Intersection(redB);

                // 4. 算面积判定
                HTuple area = red.AreaCenter(out HTuple row, out HTuple col);
                int redArea = area.I;
                return redArea > 600 ? "OK" : "NG";
            }
            catch (Exception ex)
            {
                return "视觉报错：" + ex.Message;
            }
            finally
            {
                if (img != null) img.Dispose();
            }
        }
    }
}
