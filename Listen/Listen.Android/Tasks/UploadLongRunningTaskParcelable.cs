using Android.OS;
using Android.Runtime;
using Java.Interop;
using Listen.Models.Tasks;

namespace Listen.Droid.Tasks
{
    public class UploadLongRunningTaskParcelable : Java.Lang.Object, IParcelable
    {
        private static readonly GenericParcelableCreator<UploadLongRunningTaskParcelable> _creator
            = new GenericParcelableCreator<UploadLongRunningTaskParcelable>((parcel) => new UploadLongRunningTaskParcelable(parcel));

        [ExportField("CREATOR")]
        public static GenericParcelableCreator<UploadLongRunningTaskParcelable> GetCreator()
        {
            return _creator;
        }

        public UploadLongRunningTask Task { get; set; }

        public UploadLongRunningTaskParcelable()
        {
        }

        private UploadLongRunningTaskParcelable(Parcel parcel)
        {
            Task = new UploadLongRunningTask();
        }

        public int DescribeContents()
        {
            return 0;
        }

        public void WriteToParcel(Parcel dest, [GeneratedEnum] ParcelableWriteFlags flags)
        {

        }
    }
}
