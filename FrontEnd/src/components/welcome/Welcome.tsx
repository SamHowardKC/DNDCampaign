import { Link } from "react-router-dom";

export default function Welcome() {
    return (
        <div className="welcome">
            <h1>Welcome to the DnD Campaign Manager!</h1>
            <h2>This is a personal project that I've made for fun!</h2>
            <h2>This is a work in progress, it is not complete yet, it does not function as a product as of yet.</h2>
            <h2>It is NOT a secure project so DO NOT use real data you use for other purposes such as passwords, usernames and email addresses.</h2>
            <h3>This project uses the free tier of an API service, therefore the API can take 30-60 seconds to spin up on first use.</h3>
            <h3>I have thoroughly enjoyed working on this project and I hope you enjoy using it as well!</h3> 
            <h4>Please look at my GitHub profile for more information.</h4>
            <h4>https://github.com/SamHowardKC/DNDCampaign</h4>
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
