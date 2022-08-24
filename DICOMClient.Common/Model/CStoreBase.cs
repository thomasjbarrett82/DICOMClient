using DICOMClient.Common.Constant;

namespace DICOMClient.Common.Model {

    public class CStoreBase : DimseService {
        protected FileFolder? _fileFolderRadio;
        public FileFolder? FileFolderRadio {
            get => _fileFolderRadio;
            set {
                _fileFolderRadio = value;
            }
        }

        protected string _fileName = "";
        public string FileName {
            get => _fileName;
            set {
                _fileName = value;
            }
        }

        protected string _folderName = "";
        public string FolderName {
            get => _folderName;
            set {
                _folderName = value;
            }
        }

        protected bool _includeSubfolders = false;
        public bool IncludeSubfolders {
            get => _includeSubfolders;
            set {
                _includeSubfolders = value;
            }
        }
    }
}
