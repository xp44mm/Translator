module Cuisl.BitmapImage
open System.IO
open System.Windows.Media.Imaging

///将图像格式转换为数据库存储的字节格式
let toBytes(image:BitmapImage) =
    use stream = new MemoryStream()
    let encoder = new PngBitmapEncoder()
    encoder.Frames.Add(BitmapFrame.Create(image))
    encoder.Save(stream)
    stream.Seek(0L, SeekOrigin.Begin)|> ignore
    use reader = new BinaryReader(stream)
    reader.ReadBytes(int stream.Length)

///将数据库存储的字节格式转换为图像格式
let fromBytes (bytes:byte[])=
    if bytes.Length > 0 then
        let image = new BitmapImage()
        image.BeginInit()
        image.StreamSource <- new MemoryStream(bytes)
        image.EndInit()
        image
    else
        new BitmapImage()
