import { Link } from "react-router-dom";

export default function Welcome() {
    return (
        <div className="welcome">
            <h1>Welcome to the DnD Campaign Manager!</h1>
            <h2>This is a personal project that I've made for fun!</h2>
            <h2>This is a work in progress, it is not complete yet, it does not function as a product as of yet.</h2>
            <h2>“While reasonable security measures are in place, this project is not intended for storing sensitive or confidential information. so DO NOT use real data you use for other purposes such as passwords, usernames and email addresses.</h2>
            <h3>This project uses the free tier of an API hosting service, therefore the API can take 30-60 seconds to spin up on first use.</h3>
            <h3>I have thoroughly enjoyed working on this project and I hope you enjoy using it as well!</h3> 
            <h4>Please look at my GitHub profile for more information.</h4>
            <h4>
            <a 
                href="https://github.com/SamHowardKC/DNDCampaign" 
                target="_blank" 
                rel="noopener noreferrer"
            >
                https://github.com/SamHowardKC/DNDCampaign
            </a>
            </h4>
            <h5>Click the below link to view the privacy policy:</h5>
            <h5>
            <a 
                href="https://github.com/SamHowardKC/DNDCampaign/blob/main/PRIVACY.md" 
                target="_blank" 
                rel="noopener noreferrer"
            >
                https://github.com/SamHowardKC/DNDCampaign/blob/main/PRIVACY.md
            </a>
            </h5>
            <p>Please login or register to continue.</p>

            <div style={{ marginTop: "20px" }}>
                <Link to="/login">
                    <button style={{
                        padding: "10px 20px",
                        fontSize: "1rem",
                        cursor: "pointer"
                    }}>
                        Login
                    </button>
                </Link>
            </div>
        </div>
    );
}
