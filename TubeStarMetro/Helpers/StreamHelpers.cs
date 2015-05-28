using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace TubeStarMetro
{
    public static class StreamHelpers
    {
        public static byte[] StreamToBytes(Stream source)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                source.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static Stream BytesToStream(byte[] source)
        {
            return new MemoryStream(source);
        }

        public async static void SetImageFromByteArray(this Image image, byte[] data)
        {
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(stream))
                {
                    writer.WriteBytes(data);
                    await writer.StoreAsync();
                    await writer.FlushAsync();
                    writer.DetachStream();
                }

                stream.Seek(0);

                BitmapImage bitmap = new BitmapImage();
                bitmap.SetSource(stream);

                image.Source = bitmap;
            }
        }
    }
}