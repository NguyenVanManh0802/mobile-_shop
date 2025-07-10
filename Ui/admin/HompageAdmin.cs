using MobileShop.dataAccessLayer.admin;
using MobileShop.models;
using MobileShop.Ui.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MobileShop.Ui.admin
{
    public partial class HompageAdmin : Form
    {

        private string _selectedMobileImagePath; //

        // Biến để lưu trữ IMEI của Mobile đang được chọn
        private string _selectedMobileImeiNo = null; //
        private int _selectedEmployeeId = -1;

        private string _selectedModelIdForStock = null; // MỚI: Biến này dùng để theo dõi ModelId được chọn trong Stock tab
        public HompageAdmin()
        {
            InitializeComponent();
            LoadCompaniesData();
        }

        private Color activeColor = Color.DodgerBlue;
        private Color inactiveColor = Color.Transparent;

        private void SetActiveLabel(Label activeLabel)
        {
            foreach (Control control in panelMenu.Controls)
            {
                if (control is Label)
                {
                    control.BackColor = inactiveColor;
                }
            }
            activeLabel.BackColor = activeColor;
        }

        private void lbCompany_Click(object sender, EventArgs e)
        {
            panelCompany.BringToFront();
            SetActiveLabel(lbCompany);
            LoadCompaniesData(); // Tải lại dữ liệu khi nhấp vào tab Company
        }

        private void HompageAdmin_Load(object sender, EventArgs e)
        {
            panelCompany.BringToFront();
            SetActiveLabel(lbCompany);
            LoadCompaniesData(); // Tải dữ liệu khi form được load
        }

        private void lbModel_Click(object sender, EventArgs e)
        {
            panelModel.BringToFront();
            SetActiveLabel(lbModel);
            LoadModelsData(); // MỚI: Tải dữ liệu Model khi nhấp vào tab Model
            LoadCompaniesToComboBox(); // MỚI: Tải danh sách công ty vào combobox (nếu có)
        }
        private void labelMobile_Click(object sender, EventArgs e)
        {
            panelMobile.BringToFront();
            SetActiveLabel(labelMobile);
            LoadModelsToMobileComboBox();
            LoadMobilesData();  


        }
        private void lbStock_Click(object sender, EventArgs e)
        {
            panelStock.BringToFront();
            SetActiveLabel(lbStock);
            LoadTransactionsData();
            LoadModelsToStockComboBox();
        }

        private void lbEmployee_Click(object sender, EventArgs e)
        {
            panelEmployee.BringToFront();
            SetActiveLabel(lbEmployee);
            LoadEmployeesData();
        }

        private void lbReport_Click(object sender, EventArgs e)
        {
            panelReport.BringToFront();
            SetActiveLabel(lbReport);
        }

        private void label14_Click(object sender, EventArgs e)
        {
            // Empty click handler
        }

        //add company in database
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            companyData companyDAL = new companyData();
            string companyName = textBoxNameCompany.Text.Trim();

            if (!string.IsNullOrEmpty(companyName))
            {
                Company newCompany = new Company { CName = companyName };
                if (companyDAL.AddCompany(newCompany))
                {
                    MessageBox.Show("Company added successfully!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxNameCompany.Clear();
                    LoadCompaniesData(); // Tải lại dữ liệu vào DataGridView sau khi thêm thành công
                    LoadCompaniesToComboBox(); // Cập nhật lại ComboBox Model
                }
                else
                {
                    MessageBox.Show("Failed to add company. Check logs for details.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a company name.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Hàm để tải dữ liệu công ty vào DataGridView
        private void LoadCompaniesData()
        {
            companyData companyDAL = new companyData();
            try
            {
                List<Company> companies = companyDAL.GetAllCompanies();
                dataGridViewCompany.DataSource = companies;

                dataGridViewCompany.Columns["CompId"].HeaderText = "Mã Công ty";
                dataGridViewCompany.Columns["CName"].HeaderText = "Tên Công ty";
                dataGridViewCompany.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading company data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Error loading company data: " + ex.Message);
            }
        }

        // Xử lý sự kiện click nút Delete Company
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewCompany.SelectedRows.Count > 0)
            {
                string comIdToDelete = dataGridViewCompany.SelectedRows[0].Cells["CompId"].Value.ToString();
                string comNameToDelete = dataGridViewCompany.SelectedRows[0].Cells["CName"].Value.ToString();

                DialogResult confirmResult = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa công ty '{comNameToDelete}' (ID: {comIdToDelete}) không?",
                    "Xác nhận xóa Công ty",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmResult == DialogResult.Yes)
                {
                    companyData companyDAL = new companyData();
                    if (companyDAL.DeleteCompany(comIdToDelete))
                    {
                        MessageBox.Show("Xóa công ty thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCompaniesData(); // Tải lại dữ liệu sau khi xóa thành công
                        LoadCompaniesToComboBox(); // Cập nhật lại ComboBox Model
                        LoadModelsData(); // Cập nhật DataGridView Model (nếu có model nào thuộc cty bị xóa)
                    }
                    else
                    {
                        // Lỗi đã được hiển thị từ trong companyDAL.DeleteCompany()
                        // Không cần MessageBox ở đây nữa, hoặc chỉ cần thông báo chung nếu muốn
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một công ty để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // =====================================================================
        // CÁC HÀM XỬ LÝ CHO MODEL
        // =====================================================================

        // Hàm để tải dữ liệu Model vào DataGridView
        private void LoadModelsData()
        {
            ModelData modelDAL = new ModelData();
            try
            {
                List<Model> models = modelDAL.GetAllModels();
                dataGridViewModel.DataSource = models; // Đảm bảo bạn có DataGridView tên là dataGridViewModel trên panelModel

                // Tùy chỉnh hiển thị cột nếu cần
                dataGridViewModel.Columns["ModelId"].HeaderText = "Mã Model";
                dataGridViewModel.Columns["CompId"].HeaderText = "Mã Công ty";
                dataGridViewModel.Columns["ModelNum"].HeaderText = "Số Model";
                dataGridViewModel.Columns["AvailableQty"].HeaderText = "Số lượng có sẵn";
                dataGridViewModel.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading model data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Error loading model data: " + ex.Message);
            }
        }

        // Hàm để tải danh sách công ty vào ComboBox (ví dụ: comboBoxCompId)
        private void LoadCompaniesToComboBox()
        {
            companyData companyDAL = new companyData();
            try
            {
                List<Company> companies = companyDAL.GetAllCompanies();
                comboBoxComId.DataSource = companies; // Đảm bảo bạn có ComboBox tên là comboBoxCompId trên panelModel
                comboBoxComId.DisplayMember = "CName"; // Hiển thị tên công ty
                comboBoxComId.ValueMember = "CompId"; // Giá trị thực tế là mã công ty
                comboBoxComId.SelectedIndex = -1; // Chọn không có gì ban đầu
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading companies for ComboBox: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Error loading companies for ComboBox: " + ex.Message);
            }
        }



        // Xử lý sự kiện click nút Delete Model
        private void buttonDeleteModel_Click(object sender, EventArgs e)
        {
            if (dataGridViewModel.SelectedRows.Count > 0)
            {
                string modelIdToDelete = dataGridViewModel.SelectedRows[0].Cells["ModelId"].Value.ToString();
                string modelNumToDelete = dataGridViewModel.SelectedRows[0].Cells["ModelNum"].Value.ToString();

                DialogResult confirmResult = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa Model '{modelNumToDelete}' (ID: {modelIdToDelete}) không?",
                    "Xác nhận xóa Model",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmResult == DialogResult.Yes)
                {
                    ModelData modelDAL = new ModelData();
                    if (modelDAL.DeleteModel(modelIdToDelete))
                    {
                        MessageBox.Show("Xóa Model thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadModelsData(); // Tải lại dữ liệu sau khi xóa thành công
                    }
                    else
                    {
                        // Lỗi đã được hiển thị từ trong ModelData.DeleteModel()
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một Model để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ModelData modelDAL = new ModelData();
            string modelNumber = textBoxModelNum.Text.Trim(); 
            string selectedCompId = null;

            // Kiểm tra ComboBox nếu nó được sử dụng
            if (comboBoxComId.SelectedValue != null) 
            {
                selectedCompId = comboBoxComId.SelectedValue.ToString();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một công ty.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int availableQty = 0;
            // Đảm bảo bạn có NumericUpDown tên là numericUpDownAvailableQty hoặc TextBox tên là textBoxAvailableQty
            if (!int.TryParse(textBoxAvailable.Text, out availableQty))
            {
                MessageBox.Show("Số lượng phải là một số nguyên hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (!string.IsNullOrEmpty(modelNumber) && !string.IsNullOrEmpty(selectedCompId))
            {
                Model newModel = new Model
                {
                    CompId = selectedCompId,
                    ModelNum = modelNumber,
                    AvailableQty = availableQty
                };

                if (modelDAL.AddModel(newModel))
                {
                    MessageBox.Show("Model added successfully!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxModelNum.Clear();
                    textBoxAvailable.Text = "0"; // Reset số lượng
                    comboBoxComId.SelectedIndex = -1; // Reset lựa chọn công ty
                    LoadModelsData(); // Tải lại dữ liệu vào DataGridView sau khi thêm thành công
                }
                else
                {
                    // Lỗi đã được hiển thị từ trong ModelData.AddModel()
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin Model.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void panelModel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonDeleteModel_Click_1(object sender, EventArgs e)
        {
            if (dataGridViewModel.SelectedRows.Count > 0)
            {
                string modelIdToDelete = dataGridViewModel.SelectedRows[0].Cells["ModelId"].Value.ToString();
                string modelNumToDelete = dataGridViewModel.SelectedRows[0].Cells["ModelNum"].Value.ToString();

                DialogResult confirmResult = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa Model '{modelNumToDelete}' (ID: {modelIdToDelete}) không?",
                    "Xác nhận xóa Model",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmResult == DialogResult.Yes)
                {
                    ModelData modelDAL = new ModelData();
                    if (modelDAL.DeleteModel(modelIdToDelete))
                    {
                        MessageBox.Show("Xóa Model thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadModelsData(); // Tải lại dữ liệu sau khi xóa thành công
                    }
                    else
                    {
                        // Lỗi đã được hiển thị từ trong ModelData.DeleteModel()
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một Model để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        // add employee

        // =====================================================================
        // CÁC HÀM XỬ LÝ CHO EMPLOYEE (USER)
        // =====================================================================

        // Hàm để tải dữ liệu Employee vào DataGridView
        private void LoadEmployeesData()
        {
            EmployeeData employeeDAL = new EmployeeData();
            try
            {
                List<User> employees = employeeDAL.GetAllEmployees();
                // Đảm bảo bạn có DataGridView tên là dataGridViewEmployee trên panelEmployee
                dataGridViewEmployee.DataSource = employees;

                // Tùy chỉnh hiển thị cột nếu cần (dựa trên các cột trong ảnh database)
                dataGridViewEmployee.Columns["UserId"].HeaderText = "Mã NV";
                dataGridViewEmployee.Columns["Username"].HeaderText = "Tên đăng nhập";
                dataGridViewEmployee.Columns["Email"].HeaderText = "Email";
                dataGridViewEmployee.Columns["Password"].HeaderText = "Mật khẩu";
                dataGridViewEmployee.Columns["NumberPhone"].HeaderText = "SĐT";
                dataGridViewEmployee.Columns["Address"].HeaderText = "Địa chỉ";
                dataGridViewEmployee.Columns["FullName"].HeaderText = "Họ và tên";

                // Có thể ẩn cột mật khẩu nếu không muốn hiển thị trên UI
                dataGridViewEmployee.Columns["Password"].Visible = false;

                dataGridViewEmployee.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading employee data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Error loading employee data: " + ex.Message);
            }
        }


      

        // Xử lý sự kiện click nút Add Employee
        private void btnThêm_Click(object sender, EventArgs e)
        {
            EmployeeData employeeDAL = new EmployeeData();

            string username = textBoxUserName.Text.Trim(); // From image: textBoxUserName
            string email = textBoxEmail.Text.Trim();       // From image: textBoxEmail
            string password = textBoxPassword.Text;         // From image: textBoxPassword (don't trim password normally)
            string numberPhone = textBoxNumberPhone.Text.Trim(); // From image: textBoxNumberPhone
            string address = textBoxAddress.Text.Trim();   // From image: textBoxAddress
            string fullName = textBoxFullName.Text.Trim(); // From image: textBoxFullName

            // Validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các thông tin bắt buộc (Tên đăng nhập, Email, Mật khẩu, Họ và tên).", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check for uniqueness
            if (!employeeDAL.IsUsernameUnique(username))
            {
                MessageBox.Show("Tên đăng nhập này đã tồn tại. Vui lòng chọn tên khác.", "Lỗi trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!employeeDAL.IsEmailUnique(email))
            {
                MessageBox.Show("Email này đã tồn tại. Vui lòng sử dụng email khác.", "Lỗi trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create new User object
            User newUser = new User
            {
                UserName = username,
                email = email,
                Password = password,
                numberPhone = numberPhone,
                Address = address,
                fullName = fullName
            };

            // Add employee to database
            if (employeeDAL.AddEmployee(newUser))
            {
                MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Clear input fields
                textBoxUserName.Clear();
                textBoxEmail.Clear();
                textBoxPassword.Clear();
                textBoxNumberPhone.Clear();
                textBoxAddress.Clear();
                textBoxFullName.Clear();
                LoadEmployeesData(); // Refresh DataGridView
            }
            else
            {
                // Error message already shown by DAL
            }
        }

        //xóa nhân viên
        private void buttonDele_Click(object sender, EventArgs e)
        {
            // Make sure your DataGridView for Employees is named dataGridViewEmployee
            if (dataGridViewEmployee.SelectedRows.Count > 0)
            {
                // Assuming 'UserId' is the column containing the ID in the DataGridView
                string userIdToDelete = dataGridViewEmployee.SelectedRows[0].Cells["UserId"].Value.ToString();
                string usernameToDelete = dataGridViewEmployee.SelectedRows[0].Cells["Username"].Value.ToString();

                DialogResult confirmResult = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa nhân viên '{usernameToDelete}' (ID: {userIdToDelete}) không?",
                    "Xác nhận xóa Nhân viên",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmResult == DialogResult.Yes)
                {
                    EmployeeData employeeDAL = new EmployeeData();
                    if (employeeDAL.DeleteEmployee(userIdToDelete))
                    {
                        MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadEmployeesData(); // Refresh DataGridView
                    }
                    else
                    {
                        // Error message already shown by DAL
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }



        // =====================================================================
        // CÁC HÀM XỬ LÝ CHO Mobile
        // =====================================================================

        // Hàm để tải dữ liệu Mobile vào DataGridView
        private void LoadMobilesData() //
        {
            MobileData mobileDAL = new MobileData(); //
            try
            {
                List<Mobile> mobiles = mobileDAL.GetAllMobiles(); //
                dataGridViewMobile.DataSource = mobiles; //

                // Tùy chỉnh hiển thị cột
                dataGridViewMobile.Columns["ImeiNo"].HeaderText = "IMEI"; //
                dataGridViewMobile.Columns["ModelId"].HeaderText = "Mã Model"; //
                dataGridViewMobile.Columns["Status"].HeaderText = "Trạng thái"; //
                dataGridViewMobile.Columns["Price"].HeaderText = "Giá"; //
                dataGridViewMobile.Columns["Image"].HeaderText = "Đường dẫn ảnh"; //
                dataGridViewMobile.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells); //
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading mobile data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //
                Console.WriteLine("Error loading mobile data: " + ex.Message); //
            }
        }

        // Hàm để tải danh sách Model vào ComboBox (cho việc thêm/sửa Mobile)
        private void LoadModelsToMobileComboBox() //
        {
            ModelData modelDAL = new ModelData(); //
            try
            {
                List<Model> models = modelDAL.GetAllModels(); //
                comboBoxModelId.DataSource = models; // 
                comboBoxModelId.DisplayMember = "ModelNum"; //
                comboBoxModelId.ValueMember = "ModelId"; //
                comboBoxModelId.SelectedIndex = -1; //
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading models for Mobile ComboBox: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //
                Console.WriteLine("Error loading models for Mobile ComboBox: " + ex.Message); //
            }
        }

        // Hàm để xóa trắng các trường nhập liệu Mobile
        private void ClearMobileInputFields() //
        {
            
            comboBoxModelId.SelectedIndex = -1; // Tên từ ảnh: comboBoxModelId
            textBoxStatus.Clear();
            textBoxPrice.Clear();
            pictureBoxImage.Image = null; // Tên từ ảnh: pictureBoxImage
            _selectedMobileImagePath = null;
            _selectedMobileImeiNo = null;
        }

        private void buttonAddMobile_Click_1(object sender, EventArgs e)
        {
            MobileData mobileDAL = new MobileData(); //

            string modelId = null; //
            if (comboBoxModelId.SelectedValue != null) // SỬA: comboBoxMobileModelId -> comboBoxModelId
            {
                modelId = comboBoxModelId.SelectedValue.ToString(); // SỬA: comboBoxMobileModelId -> comboBoxModelId
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một Model.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning); //
                return; //
            }

            string status = textBoxStatus.Text.Trim(); //
            float price = 0; //
            if (!float.TryParse(textBoxPrice.Text.Trim(), out price)) //
            {
                MessageBox.Show("Giá phải là một số hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning); //
                return; //
            }

            // Validation cơ bản
            if (string.IsNullOrEmpty(modelId) || string.IsNullOrEmpty(status)) //
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã Model và Trạng thái.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning); //
                return; //
            }

            Mobile newMobile = new Mobile //
            {
                ModelId = modelId, //
                Status = status, //
                Price = price, //
                Image = _selectedMobileImagePath // Lưu đường dẫn ảnh
            }; //

            if (mobileDAL.AddMobile(newMobile)) //
            {
                MessageBox.Show("Thêm điện thoại thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); //
                ClearMobileInputFields(); //
                LoadMobilesData(); // Tải lại dữ liệu
            }
            else
            {
                // Error message already shown by DAL
            }
        }

        private void btnBrowseImage_Click_1(object sender, EventArgs e)
        {
            // openFileDialogMobileImage là tên OpenFileDialog bạn đã thêm vào Designer
            if (openFileDialogMobileImage.ShowDialog() == DialogResult.OK) //
            {
                try
                {
                    _selectedMobileImagePath = openFileDialogMobileImage.FileName; //
                    // Hiển thị ảnh trong PictureBox
                    pictureBoxImage.Image = new Bitmap(_selectedMobileImagePath); //
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); //
                    _selectedMobileImagePath = null; //
                    pictureBoxImage.Image = null; //
                }
            }
        }

    

        private void buttonDeleteMobile_Click_1(object sender, EventArgs e)
        {
            if (dataGridViewMobile.SelectedRows.Count > 0) //
            {
                string imeiNoToDelete = dataGridViewMobile.SelectedRows[0].Cells["ImeiNo"].Value.ToString(); //
                string modelNumOfMobile = dataGridViewMobile.SelectedRows[0].Cells["ModelId"].Value.ToString(); // Hiển thị model thay vì IMEI cho dễ nhìn

                DialogResult confirmResult = MessageBox.Show( //
                    $"Bạn có chắc chắn muốn xóa điện thoại IMEI '{imeiNoToDelete}' (Model: {modelNumOfMobile}) không?", //
                    "Xác nhận xóa Điện thoại", //
                    MessageBoxButtons.YesNo, //
                    MessageBoxIcon.Question //
                ); //

                if (confirmResult == DialogResult.Yes) //
                {
                    MobileData mobileDAL = new MobileData(); //
                    if (mobileDAL.DeleteMobile(imeiNoToDelete)) //
                    {
                        MessageBox.Show("Xóa điện thoại thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); //
                        LoadMobilesData(); //
                        ClearMobileInputFields(); //
                    }
                    else
                    {
                        // Error message already shown by DAL
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một điện thoại để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); //
            }
        }
        // =====================================================================
        // CÁC HÀM XỬ LÝ CHO STOCK / TRANSACTION
        // =====================================================================

        // Hàm để tải danh sách Model vào ComboBox cho Stock (guna2ComboBoxModelId)
        private void LoadModelsToStockComboBox()
        {
            ModelData modelDAL = new ModelData();
            try
            {
                List<Model> models = modelDAL.GetAllModels();
                guna2ComboBoxModelId.DataSource = models; // Tên từ ảnh: guna2ComboBoxModellId
                guna2ComboBoxModelId.DisplayMember = "ModelNum"; // Hiển thị số model
                guna2ComboBoxModelId.ValueMember = "ModelId";   // Giá trị thực tế là mã model
                guna2ComboBoxModelId.SelectedIndex = -1; // Chọn không có gì ban đầu
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading models for Stock ComboBox: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Error loading models for Stock ComboBox: " + ex.Message);
            }
        }

        // Hàm để tải dữ liệu Transaction vào DataGridViewStock
        private void LoadTransactionsData()
        {
            transactionData transDAL = new transactionData(); // Khởi tạo Transaction DAL
            try
            {
                List<Transaction> transactions = transDAL.GetAllTransactions();

                // Tạo DataTable để thêm cột STT
                DataTable dt = new DataTable();
                dt.Columns.Add("STT", typeof(int));
                dt.Columns.Add("Mã Giao dịch", typeof(string));
                dt.Columns.Add("Mã Model", typeof(string));
                dt.Columns.Add("Số lượng", typeof(int));
                dt.Columns.Add("Ngày", typeof(DateTime));
                dt.Columns.Add("Tổng tiền", typeof(float)); // Tên cột bạn muốn hiển thị
                dt.Columns.Add("Loại Giao dịch", typeof(string)); // Cột loại giao dịch

                int stt = 1;
                foreach (Transaction trans in transactions)
                {
                    dt.Rows.Add(stt, trans.TransId, trans.ModelId, trans.Quantity, trans.Date, trans.Amount, trans.TransactionType);
                    stt++;
                }

                dataGridViewStock.DataSource = dt; // Gán DataTable làm DataSource

                // Tùy chỉnh hiển thị cột nếu cần (tên cột đã đặt trong DataTable)
                dataGridViewStock.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading transaction data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Error loading transaction data: " + ex.Message);
            }
        }

        // Hàm để xóa trắng các trường nhập liệu Stock
        private void ClearStockInputFields()
        {
            guna2ComboBoxModelId.SelectedIndex = -1; // Tên từ ảnh: guna2ComboBoxModellId
            textBoxQuantity.Clear();
            guna2DateTimePicker1.Value = DateTime.Now; // Tên từ ảnh: guna2DateTimePicker1
            textBoxTotal.Clear(); // Tên từ ảnh: textBoxTotal
            radioButtonSale.Checked = true; // Mặc định chọn Sale khi xóa
            radioButtonPurchase.Checked = false;
            _selectedModelIdForStock = null; // Reset biến theo dõi ModelId
        }

        // Sự kiện khi Model được chọn trong ComboBox của Stock
        private void guna2ComboBoxModellId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBoxModelId.SelectedValue != null)
            {
                _selectedModelIdForStock = guna2ComboBoxModelId.SelectedValue.ToString();
            }
            else
            {
                _selectedModelIdForStock = null;
            }
            UpdateCalculatedAmount(); // Cập nhật Amount khi Model thay đổi
        }

        // Sự kiện khi số lượng thay đổi
        private void textBoxQuantity_TextChanged(object sender, EventArgs e)
        {
            UpdateCalculatedAmount(); // Cập nhật Amount khi Quantity thay đổi
        }

        // Sự kiện khi RadioButton thay đổi (Sale/Purchase)
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            // Tùy chọn: Có thể cập nhật Amount nếu giá bán/nhập khác nhau, hoặc chỉ để đảm bảo tính toán lại
            UpdateCalculatedAmount();
        }


        // Hàm tính toán và hiển thị Amount
        private void UpdateCalculatedAmount()
        {
            if (_selectedModelIdForStock == null)
            {
                textBoxTotal.Text = string.Empty; // Xóa Amount nếu chưa chọn Model
                return;
            }

            if (!int.TryParse(textBoxQuantity.Text.Trim(), out int quantity) || quantity <= 0)
            {
                textBoxTotal.Text = string.Empty; // Xóa Amount nếu Quantity không hợp lệ
                return;
            }

            transactionData transDAL = new transactionData();
            float modelPrice = transDAL.GetModelPrice(_selectedModelIdForStock);

            if (modelPrice > 0)
            {
                float totalAmount = modelPrice * quantity;
                textBoxTotal.Text = totalAmount.ToString("N0"); // Định dạng số cho dễ đọc, ví dụ: 1,000,000
            }
            else
            {
                textBoxTotal.Text = "0"; // Giá model không có
            }
        }


       
        // Sự kiện CellClick của DataGridViewStock (để hiển thị lên textbox khi chọn)
        private void dataGridViewStock_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridViewStock.Rows[e.RowIndex];

                // Lấy dữ liệu và điền vào các control
                // string transId = row.Cells["Mã Giao dịch"].Value.ToString(); // Nếu bạn muốn hiển thị TransId
                string modelId = row.Cells["Mã Model"].Value.ToString();
                int quantity = Convert.ToInt32(row.Cells["Số lượng"].Value);
                DateTime date = Convert.ToDateTime(row.Cells["Ngày"].Value);
                float amount = Convert.ToSingle(row.Cells["Tổng tiền"].Value);
                string transType = row.Cells["Loại Giao dịch"].Value.ToString();

                guna2ComboBoxModelId.SelectedValue = modelId;
                textBoxQuantity.Text = quantity.ToString();
                guna2DateTimePicker1.Value = date;
                textBoxTotal.Text = amount.ToString();

                if (transType == "Sale")
                {
                    radioButtonSale.Checked = true;
                }
                else if (transType == "Purchase")
                {
                    radioButtonPurchase.Checked = true;
                }

                _selectedModelIdForStock = modelId; // Cập nhật ModelId đang chọn
            }
        }

        private void buttonAddTr_Click(object sender, EventArgs e)
        {
            transactionData transDAL = new transactionData();

            string modelId = null;
            if (guna2ComboBoxModelId.SelectedValue != null)
            {
                modelId = guna2ComboBoxModelId.SelectedValue.ToString();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một Model.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int quantity = 0;
            if (!int.TryParse(textBoxQuantity.Text.Trim(), out quantity) || quantity <= 0)
            {
                MessageBox.Show("Số lượng phải là một số nguyên dương hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            float amount = 0;
            if (!float.TryParse(textBoxTotal.Text.Trim(), out amount) || amount < 0)
            {
                MessageBox.Show("Số tiền phải là một số hợp lệ (không âm).", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string transactionType = "";
            if (radioButtonSale.Checked)
            {
                transactionType = "Sale";
            }
            else if (radioButtonPurchase.Checked)
            {
                transactionType = "Purchase";
            }
            else
            {
                MessageBox.Show("Vui lòng chọn loại giao dịch (Purchase hoặc Sale).", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Transaction newTransaction = new Transaction
            {
                ModelId = modelId,
                Quantity = quantity,
                Date = guna2DateTimePicker1.Value, // Tên từ ảnh: guna2DateTimePicker1
                Amount = amount,
                TransactionType = transactionType
            };

            if (transDAL.AddTransaction(newTransaction))
            {
                MessageBox.Show($"Giao dịch '{transactionType}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearStockInputFields(); // Xóa các trường sau khi thêm
                LoadTransactionsData(); // Tải lại DataGridView Transaction
                LoadModelsData(); // Cập nhật lại số lượng tồn kho trên tab Model
            }
            else
            {
                // Lỗi đã được hiển thị từ DAL
            }
        }
        private void LoadSalesReport()
        {
            ReportData reportDAL = new ReportData();
            try
            {
                DateTime dateFrom = dateTimePickerFrom.Value;
                DateTime dateTo = dateTimePickerTo.Value;

                DataTable salesReport = reportDAL.GetSalesReport(dateFrom, dateTo);

                // Gán DataTable làm nguồn dữ liệu cho dataGridViewReport
                dataGridViewReport.DataSource = salesReport;

                // Tùy chỉnh hiển thị cột (HeaderText đã được đặt trong truy vấn SQL)
                dataGridViewReport.Columns["STT"].HeaderText = "STT";
                dataGridViewReport.Columns["TenKhachHang"].HeaderText = "Tên Khách hàng";
                dataGridViewReport.Columns["TenSanPham"].HeaderText = "Tên Sản phẩm";
                dataGridViewReport.Columns["GiaBan"].HeaderText = "Giá bán";
                dataGridViewReport.Columns["NgayMua"].HeaderText = "Ngày mua hàng";
                dataGridViewReport.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                // Kiểm tra nếu không có dữ liệu
                if (salesReport.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy dữ liệu bán hàng trong khoảng thời gian đã chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading sales report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Error loading sales report: " + ex.Message);
            }
        }
        private void buttonLoadData_Click(object sender, EventArgs e)
        {
            LoadSalesReport();
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            this.Hide(); // Ẩn form hiện tại
            loginAdmin loginForm = new loginAdmin(); // Tạo form đăng nhập
            loginForm.Show(); // Hiển thị form đăng nhập
        }
    }
}