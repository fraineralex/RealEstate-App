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

let btnAddImprovement = document.querySelector("#add-improvement");

btnAddImprovement.addEventListener("click", async () => {
    const { value: improvement } = await Swal.fire({
        title: "Adding Improvement",
        html: `<form method="post" action="Improvements/Create" id="frm-add-improvement">
          <label class="form-label fw-bold mt-2 float-start" for="improvement">Name</label>
          <input id="improvement" type="text" class="form-control border-secondary border border-2" placeholder="Enter the name" name="ImprovementName" required>
          <label class="form-label mt-3 fw-bold me-5 float-start" for="description">Description</label>
          <textarea id="description" type="text" class="form-control border-secondary border border-2" placeholder="Enter the description" name="ImprovementDescription" rows="4" required></textarea>
          </form>`,
        showCancelButton: true,
        focusConfirm: false,
        preConfirm: () => {
            return [document.getElementById("improvement").value];
        },
    });

    if (improvement) {
        if (improvement.filter(Boolean).length < 1) {
            Swal.fire("Error!", "The field account number can't be empty", "error");
        } else {
            let form = document.querySelector("#frm-add-improvement");
            form.submit();
        }
    }
});