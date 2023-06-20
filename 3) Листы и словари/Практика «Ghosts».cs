using System;
using System.Text;

namespace hashes
{
    public class GhostsTask :
     IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>,
     IMagic
    {
        Document document;
        Vector vector;
        Segment segment;
        Cat cat;
        Robot robot;
        readonly byte[] firstByte = Encoding.UTF8.GetBytes("DocName");
        readonly byte[] secondByte = Encoding.UTF8.GetBytes("newName");

        public static int TakeRandom()
        {
            Random rnd = new(100);
            int count = rnd.Next();
            return count;
        }

        public void DoMagic()
        {
            vector?.Add(vector);
            cat?.Rename("not cat");
            Robot.BatteryCapacity = 100;
            segment?.Start?.Add(segment.Start);
            if (document != null)
                for (int i = 0; i < firstByte.Length; i++)
                    firstByte[i] = secondByte[i];
        }

        Vector IFactory<Vector>.Create()
        {
            vector ??= new Vector(TakeRandom(), TakeRandom());
            return vector;
        }

        Segment IFactory<Segment>.Create()
        {
            segment ??= new Segment(new Vector(TakeRandom(), TakeRandom()), new Vector(TakeRandom(), TakeRandom()));
            return segment;
        }

        Document IFactory<Document>.Create()
        {
            document ??= new Document("newDoc", Encoding.UTF8, firstByte); ;
            return document;
        }

        Cat IFactory<Cat>.Create()
        {
            cat ??= new Cat("not", "cat", DateTime.Now);
            return cat;
        }

        Robot IFactory<Robot>.Create()
        {
            Robot.BatteryCapacity = TakeRandom();
            robot ??= new Robot(TakeRandom().ToString());
            return robot;
        }
    }
}