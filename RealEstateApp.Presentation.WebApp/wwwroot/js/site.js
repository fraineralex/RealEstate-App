let arrow = document.querySelectorAll(".arrow");
for (var i = 0; i < arrow.length; i++) {
    arrow[i].addEventListener("click", (e) => {
        let arrowParent = e.target.parentElement.parentElement;//selecting main parent of arrow
        arrowParent.classList.toggle("showMenu");
    });
}

let sidebar = document.querySelector(".sidebar");
let sidebarBtn = document.querySelector(".bi-building-fill-check");
console.log(sidebarBtn);
sidebarBtn.addEventListener("click", () => {
    sidebar.classList.toggle("close");
});

//let btnAddImprovement = document.querySelector("#add-improvement");

async function Create(name, controller) {

    const { value: entity } = await Swal.fire({
        title: `Adding ${name}`,
        html: `<form method="post" action="${controller}/Create" id="frm-create">
          <label class="form-label fw-bold mt-2 float-start" for="name">Name</label>
          <input id="name" type="text" class="form-control border-secondary border border-2" placeholder="Enter the name" name="Name" required>
          <label class="form-label mt-3 fw-bold me-5 float-start" for="description">Description</label>
          <textarea id="description" type="text" class="form-control border-secondary border border-2" placeholder="Enter the description" name="Description" rows="4" required></textarea>
          </form>`,
        showCancelButton: true,
        focusConfirm: false,
        preConfirm: () => {
            return [document.getElementById("name").value, document.getElementById("description").value ];
        },
    });

    if (entity) {
        if (entity.filter(Boolean).length < 2) {
            Swal.fire("Error!", "The fields can't be empty", "error");
        } else {
            let form = document.querySelector("#frm-create");
            form.submit();
        }
    }
};

function Delete(Id, Controller, name) {
    Swal.fire({
        title: `Are you sure you want to delete this ${name}?`,
        text: "Once it has been deleted it cannot be recovered.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#3085d6",
        confirmButtonText: "Delete",
        reverseButtons: true,
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire("Deleted!", `${name} deleted successfully`, "success");

            setTimeout(() => {
                let form = document.createElement("form");
                form.action = `/${Controller}/Delete`;
                form.method = "POST";

                let entityId = document.createElement("input");
                entityId.type = "hidden";
                entityId.name = "Id";
                entityId.value = `${Id}`;

                form.appendChild(entityId);

                document.body.append(form);
                form.submit();
            }, 1000);
        }
    });
}

function ChangeUserStatus(action, name, id) {
    Swal.fire({
        title: `Are you sure you want to ${action} this ${name}?`,
        text: "Make sure before acept this action",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#3085d6",
        confirmButtonText: "Change",
        reverseButtons: true,
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire("Deleted!", `${name} changed successfully`, "success");

            setTimeout(() => {
                let form = document.createElement("form");
                form.action = `/Admin/ChangeUserStatus`;
                form.method = "POST";

                let entityId = document.createElement("input");
                entityId.type = "hidden";
                entityId.name = "Id";
                entityId.value = `${id}`;

                let entityRole = document.createElement("input");
                entityRole.type = "hidden";
                entityRole.name = "Role";
                entityRole.value = `${name}`;

                form.appendChild(entityId);
                form.appendChild(entityRole);

                document.body.append(form);
                form.submit();
            }, 1000);
        }
    });
}