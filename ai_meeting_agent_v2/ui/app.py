
import streamlit as st
from main import run_agent

st.title("🤖 AI Meeting Agent (Advanced)")

transcript = st.text_area("Paste Meeting Transcript")

if st.button("Analyze"):
    result = run_agent(transcript)

    if "raw_output" in result:
        st.write(result["raw_output"])
    else:
        st.subheader("📝 Summary")
        st.write(result["summary"])

        st.subheader("📌 Key Points")
        st.write(result["key_points"])

        st.subheader("⚠️ Risks")
        st.write(result["risks"])

        st.subheader("✅ Decisions")
        st.write(result["decisions"])

        st.subheader("📋 Tasks")
        for task in result["tasks"]:
            st.write(task)
