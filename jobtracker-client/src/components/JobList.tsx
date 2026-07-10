import "../../node_modules/bootstrap/scss/bootstrap.scss";
import axios from "axios";
import { useEffect, useState } from "react";
const API = import.meta.env.VITE_API_URL;

interface Job {
    "id": string,
    "company": string,
    "role": string,
    "location": string,
    "status": string,
    "notes": string
}

export default function JobList() {
    const [jobs, setJobs] = useState<Job[]>([]);
    const [id, setId] = useState("");
    const [company, setCompany] = useState("");
    const [role, setRole] = useState("");
    const [location, setLocation] = useState("");
    const [status, setStatus] = useState("Applied");
    const [notes, setNotes] = useState("");

    async function Load() {
        const result = await axios.get(`${API}/Job/GetJob`);
        setJobs(result.data);
    }

    // Option 1: Implementing useEffect for Load
    useEffect(() => {
        (async () => await Load())();
    }, [])

    // Option 2: Implementing useEffect for Load
    // useEffect(() => {
    //     async function Load() {
    //         const result = await axios.get(`${API}/Job/GetJob`);
    //         setJobs(result.data);
    //     }
    //     Load()
    // }, [])

    async function save(event: React.MouseEvent<HTMLButtonElement>) {
        event.preventDefault();
        try {
            // await axios.post(`${API}/Job/AddJob`, {
            //     Company: company,
            //     Role: role,
            //     Location: location,
            //     Status: status,
            //     Notes: notes
            // });
            await axios.post(`${API}/Job/AddJob`, {
                company,
                role,
                location,
                status,
                notes
            });
            alert("Job successfully added!!");
            setId("");
            setCompany("");
            setRole("");
            setLocation("");
            setStatus("");
            setNotes("")
            Load();
        }
        catch(err) {
            if(axios.isAxiosError(err)) {
                console.error('Server Validation Error: ', err.response?.data);
            } else {
                console.error('Unexpected error:', err);
            }
            alert(err);
        }
    }

    async function editJob(jobs: Job) {
        setId(jobs.id);
        setCompany(jobs.company);
        setRole(jobs.role);
        setLocation(jobs.location);
        setStatus(jobs.status);
        setNotes(jobs.notes);
    }

    async function deleteJob(id: string) {
        await axios.delete(`${API}/Job/DeleteJob/${id}`)
        alert("Job was successfully deleted!!")
        setId("");
        setCompany("");
        setRole("");
        setLocation("");
        setStatus("Applied");
        setNotes("");
        Load();
    }

    async function update(event: React.SyntheticEvent) {
        event.preventDefault();

        // Ensure the id exists before making the API request
        if(!id) {
            alert("No job ID provided");
            return;
        }
        try {
            await axios.patch(`${API}/Job/UpdateJob/${id}`, {
                id,
                company,
                role,
                location,
                status,
                notes
            });

            alert("Update was successful!!");
            setId("");
            setCompany("");
            setRole("");
            setLocation("");
            setStatus("");
            setNotes("");
            Load();
        }
        catch(error: unknown) {
            console.error(error);
            const message = error instanceof Error ? error.message : "Unknown error";
            alert("catch error: " + message || "Unknown error");
        }
    }

    return(
        <>
            <section>
                <h1>Job Application Tracker</h1>
                <div className="container mt-4">
                    <form>
                        <div className="form-group">
                            <label>Company</label>
                            <input type="text" className="form-control" id="company" value={company} onChange={(event) => {
                                setCompany(event.target.value);
                            }} />
                        </div>
                        <div className="form-group">
                            <label>Position</label>
                            <input type="text" className="form-control" id="role" value={role} onChange={(event) => {
                                setRole(event.target.value);
                            }} />
                        </div>
                        <div className="form-group">
                            <label>Location</label>
                            <input type="text" className="form-control" id="location" value={location} onChange={(event) => {
                                setLocation(event.target.value);
                            }} />
                        </div>
                        <div className="form-group">
                            <label>Status</label>
                            <select className="form-control" id="status" value={status} onChange={(event) => setStatus(event.target.value)} defaultValue={status}>
                                <option value="Applied">Applied</option>
                                <option value="Offered">Offered</option>
                                <option value="Interviewing">Interviewing</option>
                                <option value="Rejected">Rejected</option>
                            </select>
                        </div>
                        <div className="form-group">
                            <label>Notes</label>
                            <textarea className="form-control" id="notes" value={notes} onChange={(event) => {
                                setNotes(event.target.value);
                            }}></textarea>
                        </div>
                        <div>
                            <button className="btn btn-primary mt-4 me-2" onClick={save}>Save</button>
                            <button className="btn btn-warning mt-4" onClick={update}>Update</button>
                        </div>
                    </form>
                </div>
            </section>
            <section className="mt-5">
                <table className="table">
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Company</th>
                            <th>Position</th>
                            <th>Location</th>
                            <th>Status</th>
                            <th>Notes</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    {jobs.map(job => {
                        return(
                            <tbody>
                                <tr key={job.id}>
                                    <th>{job.id}</th>
                                    <td>{job.company}</td>
                                    <td>{job.role}</td>
                                    <td>{job.location}</td>
                                    <td>{job.status}</td>
                                    <td>{job.notes}</td>
                                    <td>
                                        <button className="btn btn-success me-3" onClick={() => editJob(job)}>Edit</button>
                                        <button className="btn btn-danger" onClick={() => deleteJob(job.id)}>Delete</button>
                                    </td>
                                </tr>
                            </tbody>
                        )
                    })}
                    
                </table>
            </section>
        </>
    );
}